﻿namespace GenericRateLimiter.Core;

/// <summary>
/// Represents a set of rate limiters.
/// </summary>
public class CompositeRateLimiter
{
    private readonly IList<ActionRateLimiter> _rateLimiters;

    /// <summary>
    /// The last time this rate limiter set was accessed.
    /// </summary>
    public DateTime LastAccessed { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeRateLimiter"/> class.
    /// </summary>
    /// <param name="rateLimiters">The collection of rate limiters.</param>
    internal CompositeRateLimiter(IEnumerable<ActionRateLimiter> rateLimiters)
    {
        _rateLimiters = new List<ActionRateLimiter>(rateLimiters);
        LastAccessed = DateTime.UtcNow;
    }

    /// <summary>
    /// Triggers all rate limiters in the set.
    /// </summary>
    /// <returns>True if any rate limiter in the set is triggered and rate limited, otherwise false.</returns>
    public bool Trigger()
    {
        LastAccessed = DateTime.UtcNow;
        
        foreach (var rateLimiter in _rateLimiters)
        {
            if (rateLimiter.Trigger())
            {
                return true;
            }
        }

        return false;
    }
}