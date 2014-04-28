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
using Microsoft.WindowsAzure.Storage.Blob;

namespace GarbageCollector
{
    public class WorkerRole : RoleEntryPoint
    {
        CloudBlobContainer resultSetsContainer;
        const int garbageFrequency = 1800*1000;
        CloudQueue garbageQueue;
        public override void Run()
        {
            Trace.TraceInformation("GarbageCollector entry point called", "Information");

            while (true)
            {
                Thread.Sleep(garbageFrequency);
                Trace.TraceInformation("GarbageCollector starts working", "Information");
                CloudQueueMessage receivedMessage = garbageQueue.GetMessage();
                while (receivedMessage != null)
                {
                    //
                    //Removing blob
                    //
                    try
                    {
                        CloudBlockBlob cbb = resultSetsContainer.GetBlockBlobReference(receivedMessage.AsString);
                        cbb.DeleteIfExistsAsync();
                    }
                    finally
                    {
                        garbageQueue.DeleteMessage(receivedMessage);
                        Trace.TraceInformation("GarbageCollector removed the entry", "Information");
                        receivedMessage = garbageQueue.GetMessage();
                    }
                }
                Trace.TraceInformation("GarbageCollector stops working", "Information");
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
            CloudBlobClient cbc = csa.CreateCloudBlobClient();
            resultSetsContainer = cbc.GetContainerReference("resultsetscontainer");
            resultSetsContainer.CreateIfNotExists();
            return base.OnStart();
        }
    }
}
