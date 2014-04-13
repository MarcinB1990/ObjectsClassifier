using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebRole
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            //kolejki
            CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
            CloudQueueClient cqc = csa.CreateCloudQueueClient();
            CloudQueue garbageQueue = cqc.GetQueueReference("garbagequeue");
            garbageQueue.CreateIfNotExists();
            CloudQueue inputQueue = cqc.GetQueueReference("inputqueue");
            inputQueue.CreateIfNotExists();
            CloudQueue outputQueue = cqc.GetQueueReference("outputqueue");
            outputQueue.CreateIfNotExists();

            //kontenery na bloby
            CloudBlobClient cbc=csa.CreateCloudBlobClient();
            BlobContainerPermissions bcp = new BlobContainerPermissions();
            bcp.PublicAccess = BlobContainerPublicAccessType.Blob;
            CloudBlobContainer trainingSetsContainer = cbc.GetContainerReference("trainingsets");
            trainingSetsContainer.CreateIfNotExists();
            trainingSetsContainer.SetPermissions(bcp);
            CloudBlobContainer resultSetsContainer = cbc.GetContainerReference("resultgsets");
            resultSetsContainer.CreateIfNotExists();
            resultSetsContainer.SetPermissions(bcp);
            CloudBlobContainer inputFilesContainer = cbc.GetContainerReference("inputfiles");
            inputFilesContainer.CreateIfNotExists();
            inputFilesContainer.SetPermissions(bcp);

            //tabele
            CloudTableClient ctc = csa.CreateCloudTableClient();
            CloudTable trainingSets = ctc.GetTableReference("trainingsets");
            trainingSets.CreateIfNotExists();
            CloudTable resultSets = ctc.GetTableReference("resultsets");
            resultSets.CreateIfNotExists();
            return base.OnStart();
        }
    }
}
