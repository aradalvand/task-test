﻿Console.WriteLine("Here");
var r = Subscribe();
Console.WriteLine("Here now");
await foreach (var req in r)
{
    _ = Handle(req);
}

Console.ReadKey();

async Task Handle(int req)
{
    await Task.Delay(1000);
    Console.WriteLine($"{req} handled.");
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