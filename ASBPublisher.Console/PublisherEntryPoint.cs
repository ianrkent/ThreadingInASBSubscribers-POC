using System;

namespace ASBPublisher
{
    public class PublisherEntryPoint
    {
        public static void Main(string[] args)
        {
            var domainPublisher = new MessagePublisher();

            Console.Write("How many messsages to publish?  (Q to quit)");
            var messageCountInput = Console.ReadLine();

            while (messageCountInput.ToLower() != "q")
            {
                int messageCount;
                if (int.TryParse(messageCountInput, out messageCount))
                {
                    domainPublisher.Publish(messageCount);
                    Console.WriteLine($"Finished publishing { messageCount } messages");
                }
                else
                {
                    Console.WriteLine($"ERROR: Could not convert { messageCountInput } to an integer.");
                }

                Console.Write("How many messsages to publish?  (Q to quit)");
                messageCountInput = Console.ReadLine();
            }
        }
    }
}
