using System.Collections.Concurrent;
using GenericRateLimiter.Core;
using GenericRateLimiter.Core.WasteCleaners;

namespace GenericRateLimiter
{
    public class RateLimiterRepository<TId>
        where TId : notnull
    {
        private readonly ConcurrentDictionary<TId, CompositeRateLimiter> _rateLimitedEntities = new();

        public RateLimiterRepository(WasteCleanerSettings wasteCleanerSettings)
        {
            var wasteCleaner = new TimeBasedWasteCleaner<TId>(_rateLimitedEntities, wasteCleanerSettings);
            wasteCleaner.StartPeriodicCleanup();
        }
        
        public void AddOrUpdate(TId id, CompositeRateLimiter compositeRateLimiter)
        {
            _rateLimitedEntities.AddOrUpdate(id,
                _ => compositeRateLimiter,
                (_, existingValue) => existingValue);
        }

        public bool TryGet(TId id, out CompositeRateLimiter? rateLimiter)
        {
            return _rateLimitedEntities.TryGetValue(id, out rateLimiter);
        }
    }
}