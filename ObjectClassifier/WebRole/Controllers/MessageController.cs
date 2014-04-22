using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRole.Models;

namespace WebRole.Controllers
{
    public class MessageController
    {
        CloudQueue inputQueue;
        CloudQueue outputQueue;
        public MessageController()
        {
            CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
            CloudQueueClient cqc = csa.CreateCloudQueueClient();
            inputQueue = cqc.GetQueueReference("inputqueue");
            inputQueue.CreateIfNotExists();
            outputQueue = cqc.GetQueueReference("outputqueue");
            outputQueue.CreateIfNotExists();
        }
        public void SendMessage(IMessageBuilder mb,Guid operationGuid,string resultSetId,string usedUserIdToResult,bool removeResultAfterClassification,string trainingSetId,string usedUserIdToTraining, bool removeTrainingAfterClassification)
        {
            mb.BuildGuid(operationGuid);
            mb.BuildResultSetId(resultSetId);
            mb.BuildUsedUserIdToResult(usedUserIdToResult);
            mb.BuildRemoveResultAfterClassification(removeResultAfterClassification);
            mb.BuildTrainingSetId(trainingSetId);
            mb.BuildUsedUserIdToTraining(usedUserIdToTraining);
            mb.BuildRemoveTrainingAfterClassification(removeTrainingAfterClassification);
            CloudQueueMessage cqm = new CloudQueueMessage(mb.GetMessage());
            inputQueue.AddMessage(cqm);
        }
        public string ReceiveMessage()
        {
            CloudQueueMessage cqm = outputQueue.GetMessage(new TimeSpan(0, 0, 0, 0, 500));
            string s = null;
            if (cqm != null)
            {
                s = cqm.AsString;
                outputQueue.DeleteMessage(cqm);
            }
            return s;
        }
    }
}