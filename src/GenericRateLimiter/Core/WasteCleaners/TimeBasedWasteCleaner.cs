using System.Collections.Concurrent;

namespace GenericRateLimiter.Core.WasteCleaners;

internal class TimeBasedWasteCleaner<TId>(
    ConcurrentDictionary<TId, RateLimiterComposite> rateLimitedEntities,
    WasteCleanerSettings wasteCleanerSettings)
    : IWasteCleaner
    where TId : notnull
{
    public void StartPeriodicCleanup()
    {
        Task.Run(PeriodicCleanup);
    }

    private async Task PeriodicCleanup()
    {
        while (true)
        {
            await Task.Delay(wasteCleanerSettings.CleanupInterval);
            await Cleanup();
        }
        // ReSharper disable once FunctionNeverReturns
    }
    
    public Task Cleanup()
    {
        var keysToRemove = rateLimitedEntities
            .Where(x =>
                x.Value.LastAccessed + wasteCleanerSettings.MaxIdleTime < DateTime.UtcNow)
            .Select(x => x.Key)
            .ToList();
        
        foreach (var key in keysToRemove)
        {
            rateLimitedEntities.TryRemove(key, out _);
        }

        return Task.CompletedTask;
    }
}