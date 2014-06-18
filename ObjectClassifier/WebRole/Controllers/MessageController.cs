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
    /// <summary>
    /// Kontroler obsługujący kolejkę wejściową i wyjściową
    /// </summary>
    public class MessageController
    {
        private CloudQueue inputQueue;
        private CloudQueue outputQueue;

        public MessageController()
        {
            CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
            CloudQueueClient cqc = csa.CreateCloudQueueClient();
            inputQueue = cqc.GetQueueReference("inputqueue");
            inputQueue.CreateIfNotExists();
            outputQueue = cqc.GetQueueReference("outputqueue");
            outputQueue.CreateIfNotExists();
        }

        /// <summary>
        /// Metoda wstawiająca wiadomości do kolejki wejściowej
        /// </summary>
        /// <param name="mb">Implementacja interfejsu IMessageBuilder określająca sposób budowania wiadomości</param>
        /// <param name="operationGuid">Guid wiadomości</param>
        /// <param name="resultSetId">Id zbioru wynikowego</param>
        /// <param name="usedUserIdToResult">Id użytkownika dokonującego klasyfikacji</param>
        /// <param name="removeResultAfterClassification">True dla konieczności usunięcia zbioru wynikowego z pamięci po zakończeniu klasyfikacji</param>
        /// <param name="trainingSetId">Id zbioru uczącego</param>
        /// <param name="usedUserIdToTraining">Id użytkownika, który wprowadził zbiór uczący</param>
        /// <param name="removeTrainingAfterClassification"></param>
        /// <param name="methodOfClassification">True dla konieczności usunięcia zbioru uczącego z pamięci po zakończeniu klasyfikacji</param>
        public void SendInputMessage(IMessageBuilder mb,Guid operationGuid,string resultSetId,string usedUserIdToResult,bool removeResultAfterClassification,string trainingSetId,string usedUserIdToTraining, bool removeTrainingAfterClassification,int methodOfClassification,int extensionOfOutputFile)
        {
            mb.BuildGuid(operationGuid);
            mb.BuildResultSetId(resultSetId);
            mb.BuildUsedUserIdToResult(usedUserIdToResult);
            mb.BuildRemoveResultAfterClassification(removeResultAfterClassification);
            mb.BuildTrainingSetId(trainingSetId);
            mb.BuildUsedUserIdToTraining(usedUserIdToTraining);
            mb.BuildRemoveTrainingAfterClassification(removeTrainingAfterClassification);
            mb.BuildMethodOfClassification(methodOfClassification);
            mb.BuildExtensionOfOutputFile(extensionOfOutputFile);
            CloudQueueMessage cqm = new CloudQueueMessage(mb.GetMessage());
            inputQueue.AddMessage(cqm);
        }

        /// <summary>
        /// Metoda pobierająca wiadomość o określonym guid z kolejki wyjściowej
        /// </summary>
        /// <param name="operationGuid">Guid oczekiwanej wiadomości</param>
        /// <returns></returns>
        public string ReceiveMessage(Guid operationGuid)
        {
            string message = null;
            string guidExpected = operationGuid.ToString();
            bool finished=false;
            while(!finished){
                CloudQueueMessage cqm = outputQueue.GetMessage(new TimeSpan(0, 0, 0, 0, 500));
                if (cqm != null)
                {
                    if (cqm.AsString.StartsWith(guidExpected))
                    {
                        message = cqm.AsString;
                        outputQueue.DeleteMessage(cqm);
                        finished = true;
                    }
                }
            }
            return message;
        }
    }
}