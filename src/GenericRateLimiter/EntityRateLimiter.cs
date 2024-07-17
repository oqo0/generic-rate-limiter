using GenericRateLimiter.Core;
using GenericRateLimiter.Core.RateLimiters;
using GenericRateLimiter.Core.WasteCleaners;

namespace GenericRateLimiter
{
    public class EntityRateLimiter<TId> : IEntityRateLimiter<TId>
        where TId : notnull
    {
        private readonly RateLimiterRepository<TId> _rateLimiterRepository;
        private readonly IEnumerable<IRateLimiter> _rateLimiters;

        public EntityRateLimiter(IEnumerable<IRateLimiter> rateLimiters)
        {
            _rateLimiters = rateLimiters;
            _rateLimiterRepository = new RateLimiterRepository<TId>(GetWasteCleanerSettingsForLimiters());
        }

        public EntityRateLimiter(IEnumerable<ActionRateLimiter> rateLimiters, WasteCleanerSettings wasteCleanerSettings)
        {
            _rateLimiters = rateLimiters;
            _rateLimiterRepository = new RateLimiterRepository<TId>(wasteCleanerSettings);
        }
        
        public RateLimitStatus Trigger(TId id)
        {
            bool rateLimiterFound = _rateLimiterRepository.TryGet(id, out var compositeRateLimiter) &&
                                    compositeRateLimiter is not null;
            if (rateLimiterFound)
                return compositeRateLimiter!.Trigger() ? RateLimitStatus.Limited : RateLimitStatus.Accessible;
            
            var actionRateLimiters = _rateLimiters.Select(
                    rl => new ActionRateLimiter(rl.GetLimit(), rl.GetPeriod()))
                .ToList();
            
            compositeRateLimiter = new CompositeRateLimiter(actionRateLimiters);
            _rateLimiterRepository.AddOrUpdate(id, compositeRateLimiter);

            return compositeRateLimiter.Trigger() ? RateLimitStatus.Limited : RateLimitStatus.Accessible;
        }

        private WasteCleanerSettings GetWasteCleanerSettingsForLimiters()
        {
            var longestRateLimiter = _rateLimiters.Max(x => x.GetPeriod());
            return new WasteCleanerSettings(
                TimeSpan.FromMinutes(1), longestRateLimiter * 2);
        }
    }
}