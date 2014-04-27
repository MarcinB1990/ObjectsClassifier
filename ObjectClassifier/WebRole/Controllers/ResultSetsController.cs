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
    public class ResultSetsController
    {
        CloudTable resultSets;
        CloudBlobContainer inputFilesContainer;

        public ResultSetsController()
        {
            CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient ctc = csa.CreateCloudTableClient();
            resultSets = ctc.GetTableReference("resultsetstable");
            resultSets.CreateIfNotExists();
            CloudBlobClient cbc = csa.CreateCloudBlobClient();
            BlobContainerPermissions bcp = new BlobContainerPermissions();
            bcp.PublicAccess = BlobContainerPublicAccessType.Blob;
            inputFilesContainer = cbc.GetContainerReference("inputfilescontainer");
            inputFilesContainer.CreateIfNotExists();
            inputFilesContainer.SetPermissions(bcp);
        }

        public IEnumerable<ResultSetReturn> GetMyResultSets(string userId)
        {
            TableQuery<ResultSetEntity> queryGetResultSetsByUserId = new TableQuery<ResultSetEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId));
            return resultSets.ExecuteQuery(queryGetResultSetsByUserId).Select(o => new ResultSetReturn(o.NumberOfClasses,o.NumberOfAttributes,o.DateOfEntry,o.Comment,o.TrainingSetFileSource,o.InputFileSource,o.ResultSetFileSource,o.Progress)).OrderByDescending(o => o.DateOfEntry);
        }

        public string SaveNew(ResultSet resultSet,TrainingSetsController tsc)
        {
            try
            {
                string resultSetId = Guid.NewGuid().ToString();
                string referenceToInputBlob = resultSetId + "/" + resultSet.NameOfInputFile;
                CloudBlockBlob inputBlob = inputFilesContainer.GetBlockBlobReference(referenceToInputBlob);
                inputBlob.UploadFromStream(resultSet.InputFileStream);
                ResultSetEntity rse = new ResultSetEntity(resultSet.UserId, resultSetId, resultSet.NumberOfClasses, resultSet.NumberOfAttributes, DateTime.Now, resultSet.Comment, tsc.GetTrainingSetFileSourceSourceById(resultSet.UsedUserId, resultSet.TrainingSetId), inputBlob.Uri.AbsoluteUri, string.Empty, referenceToInputBlob, "in queue");
                TableOperation insertOperation = TableOperation.Insert(rse);
                resultSets.Execute(insertOperation);
                return resultSetId;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public string GetResultSetReferenceToBlobById(string userId, string resultSetId)
        {
            TableOperation selectById = TableOperation.Retrieve<ResultSetEntity>(userId, resultSetId);
            TableResult tr = resultSets.Execute(selectById);
            return ((ResultSetEntity)tr.Result).ReferenceToBlob;
        }

        public void UpadateUri(string userId, string resultSetId, string resultSetURI)
        {
            TableOperation selectById = TableOperation.Retrieve<ResultSetEntity>(userId, resultSetId);
            TableResult tr = resultSets.Execute(selectById);
            ResultSetEntity tse = ((ResultSetEntity)tr.Result);
            if (tse != null)
            {
                tse.ResultSetFileSource = resultSetURI;
                TableOperation update = TableOperation.Replace(tse);
                resultSets.Execute(update);
            }
        }
        public string GetResultSetFileNameById(string userId, string resultSetId)
        {
            TableOperation selectById = TableOperation.Retrieve<ResultSetEntity>(userId, resultSetId);
            TableResult tr = resultSets.Execute(selectById);
            string[] ifs= ((ResultSetEntity)tr.Result).InputFileSource.Split('/');
            return ifs.Last();
        }

        public void UpdateProgress(string userId, string resultSetId, string progress)
        {
            TableOperation selectById = TableOperation.Retrieve<ResultSetEntity>(userId, resultSetId);
            TableResult tr = resultSets.Execute(selectById);
            ResultSetEntity tse = ((ResultSetEntity)tr.Result);
            if (tse != null)
            {
                tse.Progress = progress;
                TableOperation update = TableOperation.Replace(tse);
                resultSets.Execute(update);
            }
        }

        public void DeleteResultSet(string userId, string resultSetId)
        {
            TableOperation rowToDelete=TableOperation.Retrieve<ResultSetEntity>(userId, resultSetId);
            TableResult tr=resultSets.Execute(rowToDelete);
            ResultSetEntity trResult = (ResultSetEntity)tr.Result;
            if (trResult != null)
            {
                TableOperation delete = TableOperation.Delete(trResult);
                resultSets.ExecuteAsync(delete);
            }
        }
    }
}