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