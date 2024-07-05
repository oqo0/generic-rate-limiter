using GenericRateLimiter.Core;
using Microsoft.Extensions.Hosting;

namespace GenericRateLimiter.Sample;

public class SampleService(IEntityRateLimiter<int> entityRateLimiter) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            var keyChar = Console.ReadKey().KeyChar;
            var rateLimitStatus = entityRateLimiter.Trigger(keyChar);

            switch (rateLimitStatus)
            {
                case RateLimitStatus.Accessible:
                    Console.WriteLine($" {DateTime.UtcNow} is Accessible");
                    break;
                case RateLimitStatus.Limited:
                    Console.WriteLine($" {DateTime.UtcNow} is LIMITED");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}