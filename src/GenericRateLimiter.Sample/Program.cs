using GenericRateLimiter.Configuration;
using GenericRateLimiter.Sample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddRateLimiter<int>(x =>
{
    x.AddRateLimiter(1, TimeSpan.FromSeconds(1));
    x.AddRateLimiter(5, TimeSpan.FromSeconds(10));
});

builder.Services.AddHostedService<SampleService>();

await builder.Build().RunAsync();



/*
var rateLimiter = new EntityRateLimiter<int>(
    new List<ActionRateLimiter>
    {
        new(1, TimeSpan.FromMilliseconds(100)),
        new(3, TimeSpan.FromSeconds(1)),
        new(30, TimeSpan.FromMinutes(1))
    },
    new WasteCleanerSettings(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10)));
    */

/*var rnd = new Random();
long counter = 0;
while (true)
{
    counter++;
    var n = rnd.Next(int.MinValue, int.MaxValue);
    var rateLimitStatus = rateLimiter.Trigger(n);

    if (counter % 1_000_000 == 0)
        Console.WriteLine(counter / 1_000_000);
}*/