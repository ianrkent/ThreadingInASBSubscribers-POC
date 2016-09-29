using System.Threading;
using Microsoft.ServiceBus.Messaging;

namespace ASBSubscriber.Worker.Subscribers
{
    public class MessageSubscriber : MessageSubscriberBase
    {
        public override void StartSubscribing(int maxConcurrency, CancellationToken cancellationToken)
        {
            var subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                ServiceBusConnectionString,
                TopicName,
                SubscriptionName);

            subscriptionClient.OnMessage(MessageHandler, new OnMessageOptions { MaxConcurrentCalls = maxConcurrency, AutoComplete = false });
        }
    }
}