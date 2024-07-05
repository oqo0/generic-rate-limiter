﻿using System.Collections.Concurrent;
using GenericRateLimiter.Core;

namespace GenericRateLimiter
{
    public class RateLimiterRepository<TId>
        where TId : notnull
    {
        private readonly ConcurrentDictionary<TId, CompositeRateLimiter> _rateLimitedEntities = new();

        public void AddOrUpdate(TId id, IEnumerable<ActionRateLimiter> rateLimiters)
        {
            var newRateLimiters = rateLimiters
                .Select(rl => new ActionRateLimiter(rl.Limit, rl.Period))
                .ToList();
            
            _rateLimitedEntities.AddOrUpdate(id,
                _ => new CompositeRateLimiter(newRateLimiters),
                (_, _) => new CompositeRateLimiter(newRateLimiters));
        }

        public bool TryGet(TId id, out CompositeRateLimiter? rateLimiter)
        {
            return _rateLimitedEntities.TryGetValue(id, out rateLimiter);
        }
    }
}