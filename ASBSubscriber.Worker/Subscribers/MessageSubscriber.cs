using System.Threading;
using Microsoft.ServiceBus.Messaging;

namespace ASBSubscriber.Worker.Subscribers
{
    public class MessageSubscriber : MessageSubscriberBase
    {
        public override void StartSubscribing(int maxConcurrency, CancellationToken cancellationToken)
        {
            var subscriptionClient = CreateSubscriptionClient();
            subscriptionClient.OnMessage(MessageHandler, GetOnMessageOptions(maxConcurrency));
        }

        public override void StartSubscribingWithAsyncHandler(int maxConcurrency, CancellationToken cancellationToken)
        {
            var subscriptionClient = CreateSubscriptionClient();
            subscriptionClient.OnMessageAsync(MessageHandlerAsync, GetOnMessageOptions(maxConcurrency));
        }

        private static OnMessageOptions GetOnMessageOptions(int maxConcurrency)
        {
            var onMessageOptions = new OnMessageOptions
            {
                MaxConcurrentCalls = maxConcurrency,
                AutoComplete = true
            };

            onMessageOptions.ExceptionReceived +=
                (sender, args) =>
                {
                    Log.Info($"Exception thrown from message pump for action {args.Action} - Exception {args.Exception}");
                };
            return onMessageOptions;
        }

        private static SubscriptionClient CreateSubscriptionClient()
        {
            var subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                ServiceBusConnectionString,
                TopicName,
                SubscriptionName);

            return subscriptionClient;
        }
    }
}