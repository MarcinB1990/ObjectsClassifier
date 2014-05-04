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
namespace Classifier
{
    public class WorkerRole : RoleEntryPoint
    {
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
                        receivedMessageParts = null;
                        resultBlockReference = string.Empty;
                        receivedMessageParts = messageBuilder.DecodeInputMessage(receivedMessage);
                        inputQueue.DeleteMessage(receivedMessage);
                        CloudBlockBlob trainingSetBlockBlob = trainingSetsContainer.GetBlockBlobReference(trainingSetsController.GetTrainingSetReferenceToBlobById(receivedMessageParts["usedUserIdToTraining"].ToString(), receivedMessageParts["trainingSetId"].ToString()));
                        string trainingSetContent = trainingSetBlockBlob.DownloadText();
                        CloudBlockBlob inputFileBlockBlob = inputFilesContainer.GetBlockBlobReference(resultSetsController.GetResultSetReferenceToBlobById(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString()));
                        string inputFileContent = inputFileBlockBlob.DownloadText();

                        TrainingSample[] trainingSamplesSet;
                        ResultSample[] resultSampleSet;
                        IResultSetBuilder resultSetBuilder;
                        string extension;
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
                        resultSetsController.UpdateProgress(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString(), "Preparing");
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
                        
                        IClassifyStrategy classifyStrategy = null;
                        switch (Int32.Parse(receivedMessageParts["methodOfClassification"].ToString()))
                        {
                            case 0:
                                classifyStrategy = new _5NNClassifier();
                                break;
                            case 1:
                                classifyStrategy = new _5NNChaudhuriClassifier();
                                break;
                            case 2:
                                classifyStrategy = new _5NNKellera();
                                break;
                        }
                        string result=classifyStrategy.Classify(trainingSamplesSet, resultSampleSet, resultSetBuilder, resultSetsController, receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString());



                        resultBlockReference = receivedMessageParts["usedUserIdToResult"].ToString() + "/result_" + resultSetsController.GetResultSetFileNameById(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString()) + extension;
                        CloudBlockBlob resultSetBlockBlob = resultSetsContainer.GetBlockBlobReference(resultBlockReference);
                        resultSetBlockBlob.UploadText(result);
                        resultSetsController.UpadateUri(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString(), resultSetBlockBlob.Uri.AbsoluteUri);

                        resultSetsController.UpdateProgress(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString(), "Completed");
                        CloudQueueMessage completeMessage = new CloudQueueMessage(receivedMessageParts["operationGuid"] + "|" + "1" + "|" + resultSetBlockBlob.Uri.AbsoluteUri + "|" + "100");
                        outputQueue.AddMessage(completeMessage, new TimeSpan(1, 0, 0));

                        Trace.TraceInformation("Classification completed", "Information");

                    }catch(Exception){
                        if (receivedMessageParts != null)
                        {
                        CloudQueueMessage completeMessage = new CloudQueueMessage(receivedMessageParts["operationGuid"] + "|" + "2" + "|" + "" + "|" + "-1");
                        outputQueue.AddMessage(completeMessage, new TimeSpan(1, 0, 0));
                        resultSetsController.UpdateProgress(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString(), "Problem");
                            }
                    }finally{
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

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
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
            trainingSetsContainer = cbc.GetContainerReference("trainingsetscontainer");
            trainingSetsContainer.CreateIfNotExists();
            trainingSetsContainer.SetPermissions(bcp);
            resultSetsContainer = cbc.GetContainerReference("resultsetscontainer");
            resultSetsContainer.CreateIfNotExists();
            resultSetsContainer.SetPermissions(bcp);
            inputFilesContainer = cbc.GetContainerReference("inputfilescontainer");
            inputFilesContainer.CreateIfNotExists();
            resultSetsContainer.SetPermissions(bcp);
            resultSetsController = new ResultSetsController();

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            return base.OnStart();
        }
    }
}
