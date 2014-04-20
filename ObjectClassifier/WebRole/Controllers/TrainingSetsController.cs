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
    public class TrainingSetsController
    {
        CloudTable trainingSets;
        CloudBlobContainer trainingSetsContainer;

        public TrainingSetsController()
        {
            CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient ctc = csa.CreateCloudTableClient();
            trainingSets = ctc.GetTableReference("trainingsetstable");
            trainingSets.CreateIfNotExists();
            CloudBlobClient cbc = csa.CreateCloudBlobClient();
            BlobContainerPermissions bcp = new BlobContainerPermissions();
            bcp.PublicAccess = BlobContainerPublicAccessType.Blob;
            trainingSetsContainer = cbc.GetContainerReference("trainingsetscontainer");
            trainingSetsContainer.CreateIfNotExists();
            trainingSetsContainer.SetPermissions(bcp);
        }

        public string SaveNew(TrainingSet trainingSet)
        {
            try
            {
                string trainingSetId = Guid.NewGuid().ToString();
                string referenceToBlob = trainingSetId + "/" + trainingSet.NameOfFile;
                CloudBlockBlob blob = trainingSetsContainer.GetBlockBlobReference(referenceToBlob);
                blob.UploadFromStream(trainingSet.FileStream);
                TrainingSetEntity tse = new TrainingSetEntity(trainingSet.UserId, trainingSetId, trainingSet.UserName, trainingSet.Name, trainingSet.NumberOfClasses, trainingSet.NumberOfAttributes,DateTime.Now, trainingSet.Comment,referenceToBlob, blob.Uri.AbsoluteUri, trainingSet.NumberOfUses);
                TableOperation insertOperation = TableOperation.Insert(tse);
                trainingSets.Execute(insertOperation);
                return trainingSetId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<TrainingSetReturn> GetMyTrainingSets(string userId)
        {
            TableQuery<TrainingSetEntity> queryGetTrainingSetsByUserId = new TableQuery<TrainingSetEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId));
            return trainingSets.ExecuteQuery(queryGetTrainingSetsByUserId).Select(o => new TrainingSetReturn(o.RowKey,o.Name, o.NumberOfClasses, o.NumberOfAttributes,o.DateOfEntry, o.Comment, o.NumberOfUses, o.TrainingSetFileSource)).OrderBy(o=>o.Name);
        }

        public string GetTrainingSetFileSourceById(string userId,string trainingSetId)
        {
            TableOperation selectById = TableOperation.Retrieve<TrainingSetEntity>(userId, trainingSetId);
            TableResult tr = trainingSets.Execute(selectById);
            return ((TrainingSetEntity)tr.Result).TrainingSetFileSource;
        }

        public bool DeleteTrainingSet(string userId,string trainingSetId)
        {
            TableOperation rowToDelete=TableOperation.Retrieve<TrainingSetEntity>(userId, trainingSetId);
            TableResult tr=trainingSets.Execute(rowToDelete);
            TrainingSetEntity trResult = (TrainingSetEntity)tr.Result;
            if (tr != null)
            {
                TableOperation delete = TableOperation.Delete(trResult);
                trainingSets.Execute(delete);
                if (trResult.NumberOfUses == 0)
                {
                    CloudBlockBlob cbb=trainingSetsContainer.GetBlockBlobReference(trResult.TrainingSetReference);
                    cbb.DeleteAsync();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void IncrementUses(string userId, string trainingSetId)
        {
            throw new NotImplementedException();
        }
    }
}