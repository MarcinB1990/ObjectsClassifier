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
        CloudBlobContainer resultSetsContainer;
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
            resultSetsContainer = cbc.GetContainerReference("resultsetscontainer");
            resultSetsContainer.CreateIfNotExists();
            resultSetsContainer.SetPermissions(bcp);
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
                string referenceToResultBlob = resultSetId + "/" + resultSet.NameOfInputFile+"_result";
                CloudBlockBlob inputBlob = inputFilesContainer.GetBlockBlobReference(referenceToInputBlob);
                inputBlob.UploadFromStream(resultSet.InputFileStream);
                ResultSetEntity rse = new ResultSetEntity(resultSet.UserId, resultSetId, resultSet.NumberOfClasses, resultSet.NumberOfAttributes, DateTime.Now, resultSet.Comment, tsc.GetTrainingSetFileSourceById(resultSet.UsedUserId, resultSet.TrainingSetId), inputBlob.Uri.AbsoluteUri, string.Empty, "To do");
                TableOperation insertOperation = TableOperation.Insert(rse);
                resultSets.Execute(insertOperation);
                return resultSetId;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}