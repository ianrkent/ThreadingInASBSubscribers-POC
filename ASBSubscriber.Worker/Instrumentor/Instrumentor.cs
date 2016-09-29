using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASBSubscriber.Worker.Instrumentor
{
    public static class Instrumentor
    {
        public static void Measure(string context, Action thingToMeasure)
        {
            var watch = Stopwatch.StartNew();
            thingToMeasure();
            watch.Stop();
            Log.Info($"{watch.ElapsedMilliseconds} ms : {context}");
        }

        public static async Task MeasureAsync(string context, Func<Task> thingToMeasure)
        {
            var watch = Stopwatch.StartNew();
            await thingToMeasure();
            watch.Stop();
            Log.Info($"{watch.ElapsedMilliseconds} ms : {context}");
        }

    }
}
