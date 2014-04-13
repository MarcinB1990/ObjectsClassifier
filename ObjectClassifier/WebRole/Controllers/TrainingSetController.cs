using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRole.Models;

namespace WebRole.Controllers
{
    public class TrainingSetController
    {
        CloudStorageAccount csa;
        CloudTableClient ctc;
        CloudTable trainingSets;
        CloudBlobClient cbc;
        CloudBlobContainer trainingSetsContainer;
        public TrainingSetController()
        {
            csa= CloudStorageAccount.DevelopmentStorageAccount;
            ctc = csa.CreateCloudTableClient();
            trainingSets = ctc.GetTableReference("trainingsetstable");
            trainingSets.CreateIfNotExists();
            cbc = csa.CreateCloudBlobClient();
            trainingSetsContainer = cbc.GetContainerReference("trainingsetscontainer");
            trainingSetsContainer.CreateIfNotExists();
        }

        public bool SaveNew(TrainingSet trainingSet)
        {
            
                string trainingSetId = Guid.NewGuid().ToString();
                CloudBlockBlob blob = trainingSetsContainer.GetBlockBlobReference(trainingSetId + "/" + trainingSet);
                blob.UploadFromStream(trainingSet.FileStream);
                TrainingSetEntity tse = new TrainingSetEntity(trainingSet.UserId, trainingSetId, trainingSet.UserName, DateTime.Now, trainingSet.Name, trainingSet.NumberOfClasses, trainingSet.NumberOfAttributes, trainingSet.Comment, blob.Uri.AbsoluteUri, 0);
                TableOperation insertOperation = TableOperation.Insert(tse);
                trainingSets.Execute(insertOperation);
                return true;
           
        }
    }
}