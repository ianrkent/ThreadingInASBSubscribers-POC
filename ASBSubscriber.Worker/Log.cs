using System.Diagnostics;
using System.Threading;

namespace ASBSubscriber.Worker
{
    public static class Log
    {
        public static void Info(string message)
        {
            Trace.TraceInformation($"Thread { Thread.CurrentThread.ManagedThreadId } : {message}");
        }
    }
}