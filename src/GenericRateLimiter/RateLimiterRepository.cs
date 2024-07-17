using System.Collections.Concurrent;
using GenericRateLimiter.Core;
using GenericRateLimiter.Core.WasteCleaners;

namespace GenericRateLimiter
{
    internal class RateLimiterRepository<TId>
        where TId : notnull
    {
        private readonly ConcurrentDictionary<TId, RateLimiterComposite> _rateLimitedEntities = new();

        public RateLimiterRepository(WasteCleanerSettings wasteCleanerSettings)
        {
            var wasteCleaner = new TimeBasedWasteCleaner<TId>(_rateLimitedEntities, wasteCleanerSettings);
            wasteCleaner.StartPeriodicCleanup();
        }
        
        public void AddOrUpdate(TId id, RateLimiterComposite rateLimiterComposite)
        {
            _rateLimitedEntities.AddOrUpdate(id,
                _ => rateLimiterComposite,
                (_, existingValue) => existingValue);
        }

        public bool TryGet(TId id, out RateLimiterComposite? rateLimiter)
        {
            return _rateLimitedEntities.TryGetValue(id, out rateLimiter);
        }
    }
}