using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ASBPublisher
{
    public class MessagePublisher
    {
        private int _messageCounter = 0;
        private const string ServiceBusConnectionString = "Endpoint=sb://martinb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=4960u1QXBG5sI7Li7VaqjnAsiBVRCLMsTGwN4z+J2/c=";
        private const int BatchSize = 20;
        private const string TopicName = "threadingtest";

        public MessagePublisher()
        {
            EnsureTopic();
        }

        private static void EnsureTopic()
        {
            var topicDescription = new TopicDescription(TopicName)
            {
                MaxSizeInMegabytes = 5120,
                DefaultMessageTimeToLive = new TimeSpan(0, 1, 0)
            };

            var namespaceManager = NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString);

            if (!namespaceManager.TopicExists(TopicName))
            {
                namespaceManager.CreateTopic(topicDescription);
            }
        }

        public void Publish(int numberOfMessages)
        {
            var topicClient = TopicClient.CreateFromConnectionString(ServiceBusConnectionString, TopicName);

            var batchOfMessages = CreateBatchOfMessages(numberOfMessages);

            while (batchOfMessages.Any())
            {
                topicClient.SendBatch(batchOfMessages);
                numberOfMessages -= batchOfMessages.Count;
                batchOfMessages = CreateBatchOfMessages(numberOfMessages);
            }
        }

        private List<BrokeredMessage> CreateBatchOfMessages(int numberOfMessagesLeftToCreate)
        {
            var numMessagesToCreate = Math.Min(BatchSize, numberOfMessagesLeftToCreate);

            var results = new List<BrokeredMessage>(numMessagesToCreate);

            for (var i = 0; i < numMessagesToCreate; i++)
            {
                results.Add(new BrokeredMessage($"Message# {_messageCounter}") { MessageId = _messageCounter.ToString()});
                _messageCounter++;
            }

            return results;
        }
    }
}