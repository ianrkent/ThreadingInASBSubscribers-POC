using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ASBSubscriber.Worker.Subscribers;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

namespace ASBSubscriber.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _completeManualResetEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Log.Info("ASBSubscriber.Worker is running");

            new MessageSubscriber().StartSubscribing(3, cancellationTokenSource.Token);

            Log.Info("Finished setting up subscription");

            _completeManualResetEvent.WaitOne();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            Log.Info("ASBSubscriber.Worker has been started");

            return true;
        }

        public override void OnStop()
        {
            Log.Info("ASBSubscriber.Worker is stopping");

            this.cancellationTokenSource.Cancel();

            base.OnStop();

            _completeManualResetEvent.Set();
            Log.Info("ASBSubscriber.Worker has stopped");
        }
    }
}
