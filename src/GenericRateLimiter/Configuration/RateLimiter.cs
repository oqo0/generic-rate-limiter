using GenericRateLimiter.Core;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRateLimiter.Configuration;

public static class RateLimiter
{
    public static IServiceCollection AddRateLimiter<TId>(
        this IServiceCollection serviceCollection,
        IEnumerable<ActionRateLimiter> rateLimiters) where TId : notnull
    {
        serviceCollection.AddSingleton(new EntityRateLimiter<TId>(rateLimiters));
        return serviceCollection;
    }
}