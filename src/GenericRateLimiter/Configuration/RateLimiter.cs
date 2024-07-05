using GenericRateLimiter.Core;
using GenericRateLimiter.Core.WasteCleaners;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRateLimiter.Configuration;

public static class RateLimiter
{
    public static IServiceCollection AddRateLimiter<TId>(
        this IServiceCollection serviceCollection,
        IEnumerable<ActionRateLimiter> rateLimiters,
        WasteCleanerSettings wasteCleanerSettings) where TId : notnull
    {
        serviceCollection.AddSingleton(new EntityRateLimiter<TId>(rateLimiters, wasteCleanerSettings));
        return serviceCollection;
    }
}