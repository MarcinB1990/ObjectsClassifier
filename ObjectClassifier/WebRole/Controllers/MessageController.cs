using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections;
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
        public void SendInputMessage(IMessageBuilder mb,Guid operationGuid,string resultSetId,string usedUserIdToResult,bool removeResultAfterClassification,string trainingSetId,string usedUserIdToTraining, bool removeTrainingAfterClassification)
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
        public string ReceiveMessage(Guid operationGuid)
        {
            string s = null;
            bool finished=false;
            while(!finished){
                CloudQueueMessage cqm = outputQueue.GetMessage(new TimeSpan(0, 0, 0, 0, 500));
                if (cqm != null)
                {
                    if (cqm.AsString.StartsWith(operationGuid.ToString()))
                    {
                    s = cqm.AsString;
                        outputQueue.DeleteMessage(cqm);
                        finished = true;
                    }
                }
            }
            return s;
        }

        public IDictionary DecodeInputMessage(CloudQueueMessage receivedMessage)
        {
            string[] decodedMessage = receivedMessage.AsString.Split('|');
            IDictionary decodedMessageDictionary = new Dictionary<string, string>();
            decodedMessageDictionary.Add("operationGuid",decodedMessage[0]);
            decodedMessageDictionary.Add("resultSetId", decodedMessage[1]);
            decodedMessageDictionary.Add("usedUserIdToResult", decodedMessage[2]);
            decodedMessageDictionary.Add("removeResultAfterClassification", decodedMessage[3]);
            decodedMessageDictionary.Add("trainingSetId", decodedMessage[4]);
            decodedMessageDictionary.Add("usedUserIdToTraining", decodedMessage[5]);
            decodedMessageDictionary.Add("removeTrainingAfterClassification", decodedMessage[6]);
            return decodedMessageDictionary;
        }
    }
}