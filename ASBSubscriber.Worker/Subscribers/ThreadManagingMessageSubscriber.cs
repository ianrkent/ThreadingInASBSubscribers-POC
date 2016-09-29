using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ASBSubscriber.Worker.Subscribers;
using Microsoft.ServiceBus.Messaging;

namespace ASBSubscriber.Worker
{
    public class ThreadManagingMessageSubscriber : MessageSubscriberBase
    {
        public static List<SubscriptionClient> SubscriptionClients { get; set; }

        public override void StartSubscribing(int maxConcurrency, CancellationToken token)
        {
            SubscriptionClients = new List<SubscriptionClient>(maxConcurrency);
            var receivers = new List<Task>(maxConcurrency);

            for (var i = 0; i < maxConcurrency; i++)
            {
                var reciever = Task.Factory.StartNew(() =>
                {
                    Log.Info("Starting a managed task to set up a subscriber");

                    var subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                        ServiceBusConnectionString,
                        TopicName,
                        SubscriptionName);

                    SubscriptionClients.Add(subscriptionClient);

                    subscriptionClient.OnMessage(MessageHandler);
                    Log.Info("Finished setting up a subcriber in a managed Task");

                }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                receivers.Add(reciever);
            }

            Task.WhenAll(receivers).Wait(token);

            Log.Info("Finished waiting for all receivers");
        }
    }
}