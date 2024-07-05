using GenericRateLimiter.Configuration.Options;
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
        serviceCollection.AddSingleton<IEntityRateLimiter<TId>, EntityRateLimiter<TId>>(_ =>
            new EntityRateLimiter<TId>(rateLimiters, wasteCleanerSettings));
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddRateLimiter<TId>(
        this IServiceCollection serviceCollection,
        Action<RateLimiterOptions> rateLimiterOptionsSetting) where TId : notnull
    {
        var rateLimiterOptions = new RateLimiterOptions();
        rateLimiterOptionsSetting.Invoke(rateLimiterOptions);
        
        serviceCollection.AddSingleton<IEntityRateLimiter<TId>, EntityRateLimiter<TId>>(_ =>
            new EntityRateLimiter<TId>(rateLimiterOptions.GetActionRateLimiters()));
        
        return serviceCollection;
    }
}