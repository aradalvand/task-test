await foreach (var req in Subscribe())
{
    await Handle(req);
}

Console.ReadKey();

async Task Handle(int req)
{
    await Task.Delay(1000);
    Console.WriteLine($"{req} handled.");
}

async IAsyncEnumerable<int> Subscribe()
{
    var random = new Random();
    while (true)
    {
        await Task.Delay(100);
        yield return random.Next(1, 100);
    }
}