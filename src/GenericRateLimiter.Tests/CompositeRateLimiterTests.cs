using GenericRateLimiter.Core;
using GenericRateLimiter.Core.RateLimiters;

namespace GenericRateLimiter.Tests;

using System;
using System.Collections.Generic;
using Xunit;

public class CompositeRateLimiterTests
{
    [Fact]
    public void Trigger_ShouldReturnLimited_WhenOneLimiterExceedsLimit()
    {
        var rateLimiters = new List<ActionRateLimiter>
        {
            new(1, TimeSpan.FromSeconds(1)),
            new(2, TimeSpan.FromSeconds(5))
        };
        var compositeRateLimiter = new CompositeRateLimiter(rateLimiters);

        Assert.False(compositeRateLimiter.Trigger());
        Assert.True(compositeRateLimiter.Trigger());
        
        Thread.Sleep(TimeSpan.FromSeconds(2));
        
        Assert.False(compositeRateLimiter.Trigger());
        Assert.True(compositeRateLimiter.Trigger());
        
        Thread.Sleep(TimeSpan.FromSeconds(1));
        
        Assert.True(compositeRateLimiter.Trigger());
    }
}
