using ALBINGIA.Framework.Common.IOFile;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Tools
{
    public static class ParallelHelper
    {
        public static void Execute(int maxDegreeOfParallelism =-1 , params Action[] actions)
        {
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }; 
            Parallel.Invoke(parallelOptions,actions);
        }

        public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> funcBody, int maxDoP = 4) {
            async Task AwaitPartition(IEnumerator<T> partition) {
                using (partition) {
                    while (partition.MoveNext()) { await funcBody(partition.Current); }
                }
            }

            return Task.WhenAll(
                Partitioner
                    .Create(source)
                    .GetPartitions(maxDoP)
                    .AsParallel()
                    .Select(p => AwaitPartition(p)));
        }
    }
}
