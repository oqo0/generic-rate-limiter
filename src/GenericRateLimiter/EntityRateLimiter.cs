using GenericRateLimiter.Core;

namespace GenericRateLimiter
{
    public class EntityRateLimiter<TId>(IEnumerable<ActionRateLimiter> rateLimiters)
        where TId : notnull
    {
        private readonly RateLimiterRepository<TId> _rateLimiterRepository = new();

        public RateLimitStatus Trigger(TId id)
        {
            bool rateLimiterFound = _rateLimiterRepository.TryGet(id, out var rateLimitersSet) &&
                                    rateLimitersSet is not null;
            if (rateLimiterFound)
                return rateLimitersSet!.Trigger() ? RateLimitStatus.Limited : RateLimitStatus.Accessible;
            
            var newRateLimiters = rateLimiters.Select(
                    rl => new ActionRateLimiter(rl.Limit, rl.Period))
                .ToList();
            
            rateLimitersSet = new CompositeRateLimiter(newRateLimiters);
            _rateLimiterRepository.AddOrUpdate(id, newRateLimiters);

            return rateLimitersSet.Trigger() ? RateLimitStatus.Limited : RateLimitStatus.Accessible;
        }
    }
}