using System.Threading;
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

        protected static void MessageHandler(BrokeredMessage message)
        {
            Log.Info($"Processing message with Id { message.MessageId }");
            message.Complete();
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