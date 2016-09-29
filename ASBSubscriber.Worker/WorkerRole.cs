using System.Net;
using System.Threading;
using ASBSubscriber.Worker.Subscribers;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace ASBSubscriber.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _completeManualResetEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Log.Info("ASBSubscriber.Worker is running");

            // new MessageSubscriber().StartSubscribingWithAsyncHandler(1, cancellationTokenSource.Token);
            new MessageSubscriber().StartSubscribing(1, cancellationTokenSource.Token);

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

            cancellationTokenSource.Cancel();

            base.OnStop();

            _completeManualResetEvent.Set();
            Log.Info("ASBSubscriber.Worker has stopped");
        }
    }
}
