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
        MessageController messageController;
        CloudBlobContainer trainingSetsContainer;
        CloudBlobContainer inputFilesContainer;
        CloudBlobContainer resultSetsContainer;

        public override void Run()
        {
            Trace.TraceInformation("Classifier entry point called", "Information");

            while (true)
            {
                Thread.Sleep(classifierFrequency);
                Trace.TraceInformation("Classifier starts working", "Information");
                CloudQueueMessage receivedMessage = inputQueue.GetMessage();
                while (receivedMessage != null)
                {
                    IDictionary receivedMessageParts = messageController.DecodeInputMessage(receivedMessage);
                    CloudBlockBlob trainingSetBlockBlob = trainingSetsContainer.GetBlockBlobReference(trainingSetsController.GetTrainingSetReferenceToBlobById(receivedMessageParts["usedUserIdToTraining"].ToString(), receivedMessageParts["trainingSetId"].ToString()));
                    string trainingSetContent = trainingSetBlockBlob.DownloadText();
                    CloudBlockBlob inputFileBlockBlob = resultSetsContainer.GetBlockBlobReference(resultSetsController.GetResultSetReferenceToBlobById(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString()));
                    string inputFileContent = inputFileBlockBlob.DownloadText();
                    string result = string.Empty;

                    //
                    //Classification process
                    result = trainingSetContent.ToUpper()+" | "+inputFileContent.ToUpper();
                    //
                    //

                    if (("1").Equals(receivedMessageParts["removeTrainingAfterClassification"].ToString()))
                    {
                        trainingSetBlockBlob.DeleteAsync();
                        trainingSetsController.DeleteTrainingSet(receivedMessageParts["usedUserIdToTraining"].ToString(), receivedMessageParts["trainingSetId"].ToString());
                    }

                    CloudBlockBlob resultSetBlockBlob = resultSetsContainer.GetBlockBlobReference(receivedMessageParts["usedUserIdToResult"].ToString() + "/result_" + resultSetsController.GetResultSetFileNameById(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString()));
                    resultSetBlockBlob.UploadText(result);
                    resultSetsController.UpadateUri(receivedMessageParts["usedUserIdToResult"].ToString(), receivedMessageParts["resultSetId"].ToString(), resultSetBlockBlob.Uri.AbsoluteUri);

                    outputQueue.AddMessage(receivedMessage);
                    inputQueue.DeleteMessage(receivedMessage);
                    Trace.TraceInformation("Classification completed", "Information");
                    receivedMessage = inputQueue.GetMessage();
                }
                Trace.TraceInformation("Classifier stops working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

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
            messageController = new MessageController();
            CloudBlobClient cbc = csa.CreateCloudBlobClient();
            BlobContainerPermissions bcp = new BlobContainerPermissions();
            bcp.PublicAccess = BlobContainerPublicAccessType.Blob;
            trainingSetsContainer = cbc.GetContainerReference("trainingsetscontainer");
            trainingSetsContainer.CreateIfNotExists();
            trainingSetsContainer.SetPermissions(bcp);
            resultSetsContainer = cbc.GetContainerReference("inputfilescontainer");
            resultSetsContainer.CreateIfNotExists();
            resultSetsContainer.SetPermissions(bcp);
            inputFilesContainer = cbc.GetContainerReference("resultsetscontainer");
            inputFilesContainer.CreateIfNotExists();
            resultSetsContainer.SetPermissions(bcp);
            resultSetsController = new ResultSetsController();
            return base.OnStart();
        }
    }
}
