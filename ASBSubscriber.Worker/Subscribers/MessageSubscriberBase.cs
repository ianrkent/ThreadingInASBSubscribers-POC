using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ASBSubscriber.Worker.Subscribers
{
    public abstract class MessageSubscriberBase
    {
        protected const string ServiceBusConnectionString = "Endpoint=sb://martinb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=4960u1QXBG5sI7Li7VaqjnAsiBVRCLMsTGwN4z+J2/c=";
        protected const string TopicName = "threadingtest";
        protected const string SubscriptionName = "ThreadingTest.Subscription";

        protected MessageSubscriberBase()
        {
            EnsureSubscription();
        }

        public abstract void StartSubscribing(int maxConcurrency, CancellationToken cancellationToken);

        public abstract void StartSubscribingWithAsyncHandler(int maxConcurrency, CancellationToken cancellationToken);

        protected static void MessageHandler(BrokeredMessage message)
        {
            Instrumentor.Instrumentor.Measure($"sync message {message.MessageId}",
                () =>
                {
                    //WaitSomethingBlocking(TimeSpan.FromMilliseconds(60));
                    //SpinThread(TimeSpan.FromMilliseconds(20));
                    //WaitSomethingBlocking(TimeSpan.FromMilliseconds(50));
                    //SpinThread(TimeSpan.FromMilliseconds(15));
                });
        }

        protected static async Task MessageHandlerAsync(BrokeredMessage message)
        {
            await Instrumentor.Instrumentor.MeasureAsync($"async message {message.MessageId}",
                async () =>
                {
                    //await AwaitSomethingAsync(TimeSpan.FromMilliseconds(60));
                    //SpinThread(TimeSpan.FromMilliseconds(20));
                    //await AwaitSomethingAsync(TimeSpan.FromMilliseconds(50));
                    //SpinThread(TimeSpan.FromMilliseconds(15));
                    //await AwaitSomethingAsync(TimeSpan.FromMilliseconds(50));

                    await Task.Delay(1);
                });
        }

        private static void WaitSomethingBlocking(TimeSpan waitTime)
        {
            Task.Delay(waitTime).Wait();
        }

        private static async Task AwaitSomethingAsync(TimeSpan awaitTime)
        {
            await Task.Delay(awaitTime);
        }

        private static void SpinThread(TimeSpan spinTime)
        {
            var now = DateTime.Now;
            while(DateTime.Now < now.Add(spinTime)) 
            {
                //Thread.Sleep(1);
            }
        }

        private static void EnsureSubscription()
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString);

            if (!namespaceManager.SubscriptionExists(TopicName, SubscriptionName))
            {
                namespaceManager.CreateSubscription(TopicName, SubscriptionName);
            }
        }
    }
}