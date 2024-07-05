using System.Collections.Concurrent;
using GenericRateLimiter.Core;

namespace GenericRateLimiter;

/// <summary>
/// A rate limiter that applies rate limiting to entities identified by <typeparamref name="TId"/>.
/// </summary>
/// <typeparam name="TId">The type of the identifier for rate limited entities.</typeparam>
public class EntityRateLimiter<TId>(IEnumerable<ActionRateLimiter> rateLimiters) : IEntityRateLimiter<TId>
    where TId : notnull
{
    private readonly ConcurrentDictionary<TId, CompositeRateLimiter> _rateLimitedEntities = new();

    /// <summary>
    /// Triggers the rate limiter for the specified entity.
    /// </summary>
    /// <param name="id">The identifier of the entity to be rate limited.</param>
    /// <returns>The rate limit status indicating whether the entity is accessible or limited.</returns>
    public RateLimitStatus Trigger(TId id)
    {
        if (_rateLimitedEntities.TryGetValue(id, out var rateLimitersSet))
            return rateLimitersSet.Trigger() ? RateLimitStatus.Limited : RateLimitStatus.Accessible;
        
        var newRateLimiters = rateLimiters.Select(
            rl => new ActionRateLimiter(rl.Limit, rl.Period))
            .ToList();
        
        rateLimitersSet = new CompositeRateLimiter(newRateLimiters);
        _rateLimitedEntities[id] = rateLimitersSet;

        return rateLimitersSet.Trigger() ? RateLimitStatus.Limited : RateLimitStatus.Accessible;
    }
}