_ = Task.Run(async () =>
{
    while (true)
    {
        await Task.Delay(1000);
        Console.WriteLine("1 second passed in task");
    }
});


while (true)
{
    await Task.Delay(1000);
    Console.WriteLine("1 second passed here");
}

Console.WriteLine("Yep");
Console.ReadLine();
