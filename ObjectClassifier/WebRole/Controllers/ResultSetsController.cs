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
        }

        public IEnumerable<ResultSetReturn> GetMyResultSets(string userId)
        {
            TableQuery<ResultSetEntity> queryGetResultSetsByUserId = new TableQuery<ResultSetEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId));
            return resultSets.ExecuteQuery(queryGetResultSetsByUserId).Select(o => new ResultSetReturn(o.Name,o.Comment,o.DateOfEntry,o.TrainingSetFileSource,o.InputFileSource,o.ResultSetFileSource,o.Progress)).OrderByDescending(o => o.DateOfEntry);
        }

    }
}