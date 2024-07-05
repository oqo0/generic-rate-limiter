using GenericRateLimiter.Core;

namespace GenericRateLimiter;

internal interface IRateLimiterRepository<in TId>
    where TId : notnull
{
    public void AddOrUpdate(TId id, IEnumerable<ActionRateLimiter> rateLimiters);
    public bool TryGet(TId id, out CompositeRateLimiter? rateLimiter);
}