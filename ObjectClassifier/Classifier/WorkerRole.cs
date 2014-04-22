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

namespace Classifier
{
    public class WorkerRole : RoleEntryPoint
    {
        const int classifierFrequency = 100;
        CloudQueue garbageQueue;
        CloudQueue inputQueue;
        CloudQueue outputQueue;
        public override void Run()
        {
            Trace.TraceInformation("Classifier entry point called", "Information");

            while (true)
            {
                Thread.Sleep(classifierFrequency);
                Trace.TraceInformation("Classifier starts working", "Information");
                CloudQueueMessage receivedMessage = inputQueue.GetMessage();
                while (receivedMessage != null)
                {
                    CloudQueueMessage cqm = new CloudQueueMessage("dupa " + receivedMessage.AsString);
                    outputQueue.AddMessage(cqm);
                    
                    //
                    //Classification process
                    //
                    Trace.TraceWarning("odebralem wiadomosc");

                    inputQueue.DeleteMessage(receivedMessage);
                    Trace.TraceInformation("Classification completed", "Information");
                    receivedMessage = inputQueue.GetMessage();
                }
                Trace.TraceInformation("Classifier stops working", "Information");
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
            inputQueue = cqc.GetQueueReference("inputqueue");
            inputQueue.CreateIfNotExists();
            outputQueue = cqc.GetQueueReference("outputqueue");
            outputQueue.CreateIfNotExists();

            return base.OnStart();
        }
    }
}
