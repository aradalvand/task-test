using System.Threading.Tasks.Dataflow;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

Console.WriteLine(TaskScheduler.Current.MaximumConcurrencyLevel);
// BenchmarkRunner.Run<ThroughputBenchmarks>();

[MemoryDiagnoser(false)]
[ShortRunJob]
public class ThroughputBenchmarks
{
    [Benchmark]
    public async Task TaskSpawning()
    {
        List<Task> tasks = [];

        await foreach (var req in Subscribe())
            tasks.Add(Handle(req));

        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task WithSemaphore()
    {
        List<Task> tasks = [];

        var semaphore = new SemaphoreSlim(int.MaxValue);
        await foreach (var req in Subscribe())
        {
            await semaphore.WaitAsync();
            tasks.Add(Handle(req).ContinueWith(_ => semaphore.Release()));
        }

        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task ActionBlock()
    {
        var workerBlock = new ActionBlock<int>(
            Handle,
            new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = int.MaxValue
            }
        );
        await foreach (var req in Subscribe())
            workerBlock.Post(req);
        workerBlock.Complete();
        await workerBlock.Completion;
    }


    [Benchmark]
    public async Task ParallelForEach()
    {
        await Parallel.ForEachAsync(Subscribe(), parallelOptions: new()
        {
            MaxDegreeOfParallelism = int.MaxValue
        }, async (req, ct) =>
        {
            await Handle(req);
        });
    }

    private static async Task Handle(int req)
    {
        await Task.Delay(1000);
    }

    private static async IAsyncEnumerable<int> Subscribe()
    {
        for (int i = 0; i < 1_000_000; i++)
            yield return i;
    }
}