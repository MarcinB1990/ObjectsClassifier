using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Collections;
using Microsoft.WindowsAzure.Storage.Blob;
using WebRole.Controllers;
using Classifier.Classifiers.Common;
using Classifier.Classifiers;
using Classifier.Classifiers.Tests;
using WebRole.Shared;
namespace Classifier
{
    public class WorkerRole : RoleEntryPoint
    {
        const int k = 5;
        const int classifierFrequency = 100;
        CloudQueue garbageQueue;
        CloudQueue inputQueue;
        CloudQueue outputQueue;
        TrainingSetsController trainingSetsController;
        ResultSetsController resultSetsController;
        IMessageBuilder messageBuilder;
        CloudBlobContainer trainingSetsContainer;
        CloudBlobContainer inputFilesContainer;
        CloudBlobContainer resultSetsContainer;

        public override void Run()
        {
            Trace.TraceInformation("Classifier entry point called", "Information");
            IDictionary receivedMessageParts = null;
            string resultBlockReference = string.Empty;
            while (true)
            {
                Thread.Sleep(classifierFrequency);
                Trace.TraceInformation("Classifier starts working", "Information");
                CloudQueueMessage receivedMessage = inputQueue.GetMessage(new TimeSpan(0,0,0,0,500));
                while (receivedMessage != null)
                {
                    try
                    {
                        TrainingSample[] trainingSamplesSet = null;
                        ResultSample[] resultSampleSet = null;
                        IResultSetBuilder resultSetBuilder=null;
                        string extension=string.Empty;
                        receivedMessageParts = null;
                        resultBlockReference = string.Empty;
                        receivedMessageParts = messageBuilder.DecodeInputMessage(receivedMessage);
                        resultSetsController.UpdateProgress(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString(), "Preparing");
                        inputQueue.DeleteMessage(receivedMessage);
                        CloudBlockBlob trainingSetBlockBlob = trainingSetsContainer.GetBlockBlobReference(trainingSetsController.GetTrainingSetReferenceToBlobById(receivedMessageParts["usedUserIdToTraining"].ToString(), receivedMessageParts["trainingSetId"].ToString()));
                        string trainingSetContent = trainingSetBlockBlob.DownloadText();
                        int methodOfClassification=Int32.Parse(receivedMessageParts["methodOfClassification"].ToString());
                        if (methodOfClassification != (int)EnumClassificationMethod.Tests)
                        {
                            CloudBlockBlob inputFileBlockBlob = inputFilesContainer.GetBlockBlobReference(resultSetsController.GetResultSetReferenceToBlobById(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString()));
                            string inputFileContent = inputFileBlockBlob.DownloadText();
                            string[] inputElements = inputFileContent.Split('\n');
                            int inputElementsLength;
                            if ("".Equals(inputElements[inputElements.Length - 1]))
                            {
                                inputElementsLength = inputElements.Length - 1;
                            }
                            else
                            {
                                inputElementsLength = inputElements.Length;
                            }
                            resultSampleSet = new ResultSample[inputElementsLength];
                            for (int i = 0; i < inputElementsLength; i++)
                            {
                                resultSampleSet[i] = new ResultSample(inputElements[i].Split('\t'));
                            }
                        }

                        switch (int.Parse(receivedMessageParts["extensionOfOutputFile"].ToString()))
                        {
                            case 0:
                                resultSetBuilder = new ResultSetBuilderTxtImpl();
                                extension = ".txt";
                                break;
                            case 1:
                                resultSetBuilder = new ResultSetBuilderCsvImpl();
                                extension = ".csv";
                                break;
                            default:
                                resultSetBuilder = new ResultSetBuilderTxtImpl();
                                extension = ".txt";
                                break;
                        }

                        string[] trainingElements = trainingSetContent.Split('\n');
                        int trainingElementsLength;
                        if ("".Equals(trainingElements[trainingElements.Length - 1]))
                        {
                            trainingElementsLength = trainingElements.Length - 1;
                        }
                        else
                        {
                            trainingElementsLength = trainingElements.Length;
                        }
                        trainingSamplesSet = new TrainingSample[trainingElementsLength];
                        for (int i = 0; i < trainingElementsLength; i++)
                        {
                            trainingSamplesSet[i] = new TrainingSample(trainingElements[i].Split('\t'));
                        }                       
                        
                        IClassifyStrategy classifyStrategy = null;
                        switch (methodOfClassification)
                        {
                            case (int)EnumClassificationMethod.KNNClassifier:
                                classifyStrategy = new KNNClassifier();
                                break;
                            case (int)EnumClassificationMethod.KNNChaudhuriClassifier:
                                classifyStrategy = new KNNChaudhuriClassifier();
                                break;
                            case (int)EnumClassificationMethod.KNNKellerClassifier:
                                classifyStrategy = new KNNKellerClassifier();
                                break;
                            case (int)EnumClassificationMethod.Tests:
                                classifyStrategy = new TestClassifiers();
                                break;
                            default:
                                classifyStrategy = new KNNClassifier();
                                break;
                        }
                        string result = classifyStrategy.Classify(trainingSamplesSet, resultSampleSet, resultSetBuilder, resultSetsController, receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString(),k);

                        resultBlockReference = receivedMessageParts["usedUserIdToResult"].ToString() + "/" + resultSetsController.GetResultSetFileNameById(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString()) + extension;
                        CloudBlockBlob resultSetBlockBlob = resultSetsContainer.GetBlockBlobReference(resultBlockReference);
                        resultSetBlockBlob.UploadText(result);
                        resultSetsController.UpadateUri(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString(), resultSetBlockBlob.Uri.AbsoluteUri);

                        resultSetsController.UpdateProgress(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString(), "Completed");
                        CloudQueueMessage completeMessage = new CloudQueueMessage(receivedMessageParts["operationGuid"] + "|" + "1" + "|" + resultSetBlockBlob.Uri.AbsoluteUri);
                        outputQueue.AddMessage(completeMessage, new TimeSpan(1, 0, 0));

                        Trace.TraceInformation("Classification completed", "Information");
                    }
                    catch (Exception)
                    {
                        if (receivedMessageParts != null)
                        {
                            CloudQueueMessage completeMessage = new CloudQueueMessage(receivedMessageParts["operationGuid"] + "|" + "0" + "|" + "");
                            outputQueue.AddMessage(completeMessage, new TimeSpan(1, 0, 0));
                            resultSetsController.UpdateProgress(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString(), "0");
                        }
                    }
                    finally
                    {
                        if (receivedMessageParts != null)
                        {
                            if (("1").Equals(receivedMessageParts["removeTrainingAfterClassification"].ToString()))
                            {
                                trainingSetsController.DeleteTrainingSet(receivedMessageParts["usedUserIdToTraining"].ToString(), receivedMessageParts["trainingSetId"].ToString());
                            }
                            if (("1").Equals(receivedMessageParts["removeResultAfterClassification"].ToString()))
                            {
                                resultSetsController.DeleteResultSet(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString());
                                CloudQueueMessage messageForGarbage = new CloudQueueMessage(resultBlockReference);
                                garbageQueue.AddMessage(messageForGarbage, null, new TimeSpan(0, 0, 1));
                            }
                        }
                        receivedMessage = inputQueue.GetMessage();
                    }
                }
                Trace.TraceInformation("Classifier stops working", "Information");
            }
        }

        public override bool OnStart()
        {
            CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
            CloudQueueClient cqc = csa.CreateCloudQueueClient();
            garbageQueue = cqc.GetQueueReference("garbagequeue");
            garbageQueue.CreateIfNotExists();
            inputQueue = cqc.GetQueueReference("inputqueue");
            inputQueue.CreateIfNotExists();
            outputQueue = cqc.GetQueueReference("outputqueue");
            outputQueue.CreateIfNotExists();
            trainingSetsController = new TrainingSetsController();
            messageBuilder = new MessageBuilder();
            CloudBlobClient cbc = csa.CreateCloudBlobClient();
            BlobContainerPermissions bcp = new BlobContainerPermissions();
            bcp.PublicAccess = BlobContainerPublicAccessType.Blob;
            trainingSetsContainer =
            cbc.GetContainerReference("trainingsetscontainer");
            trainingSetsContainer.CreateIfNotExists();
            trainingSetsContainer.SetPermissions(bcp);
            resultSetsContainer = cbc.GetContainerReference("resultsetscontainer");
            resultSetsContainer.CreateIfNotExists();
            resultSetsContainer.SetPermissions(bcp);
            inputFilesContainer = cbc.GetContainerReference("inputfilescontainer");
            inputFilesContainer.CreateIfNotExists();
            resultSetsContainer.SetPermissions(bcp);
            resultSetsController = new ResultSetsController();
            return base.OnStart();
        } 

    }
}
