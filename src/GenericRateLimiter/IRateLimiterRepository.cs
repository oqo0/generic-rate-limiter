using GenericRateLimiter.Core;
using GenericRateLimiter.Core.RateLimiters;

namespace GenericRateLimiter;

internal interface IRateLimiterRepository<in TId>
    where TId : notnull
{
    public void AddOrUpdate(TId id, IEnumerable<ActionRateLimiter> rateLimiters);
    public bool TryGet(TId id, out RateLimiterComposite? rateLimiter);
}