using System.Diagnostics;

Console.WriteLine("Here");
var r = Subscribe();
Console.WriteLine("Here now");

Stopwatch sw = new();
sw.Start();
await Parallel.ForEachAsync(r, parallelOptions: new()
{
    MaxDegreeOfParallelism = int.MaxValue
}, async (req, ct) =>
{
    await Handle(req);
});
sw.Stop();
Console.WriteLine($"Finished in {sw.Elapsed.TotalMilliseconds:N0} ms");

Console.ReadKey();

async Task Handle(int req)
{
    await Task.Delay(1000);
    // Console.WriteLine($"{req} handled.");
}

async IAsyncEnumerable<int> Subscribe()
{
    Console.WriteLine("Subscribe");
    var random = new Random();
    for (int i = 0; i < 1_000_000; i++)
    {
        // await Task.Delay(100);
        yield return i;
    }
}