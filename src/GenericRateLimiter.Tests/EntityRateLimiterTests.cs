using GenericRateLimiter.Core;
using GenericRateLimiter.Core.RateLimiters;

namespace GenericRateLimiter.Tests;

using System;
using System.Collections.Generic;
using GenericRateLimiter;
using Xunit;

public class EntityRateLimiterTests
{
    [Fact]
    public void Trigger_ShouldLimitIndividualEntities()
    {
        var rateLimiters = new List<ActionRateLimiter>
        {
            new(2, TimeSpan.FromSeconds(1)),
            new(10, TimeSpan.FromSeconds(30))
        };
        var entityRateLimiter = new EntityRateLimiter<char>(rateLimiters);

        Assert.Equal(RateLimitStatus.Accessible, entityRateLimiter.Trigger('A'));
        Assert.Equal(RateLimitStatus.Accessible, entityRateLimiter.Trigger('A'));
        Assert.Equal(RateLimitStatus.Limited, entityRateLimiter.Trigger('A'));
        Assert.Equal(RateLimitStatus.Accessible, entityRateLimiter.Trigger('B'));
    }

    [Fact]
    public void Trigger_ShouldResetLimitsAfterPeriod()
    {
        var rateLimiters = new List<ActionRateLimiter>
        {
            new(1, TimeSpan.FromSeconds(1)),
            new(10, TimeSpan.FromSeconds(30))
        };
        var entityRateLimiter = new EntityRateLimiter<char>(rateLimiters);

        Assert.Equal(RateLimitStatus.Accessible, entityRateLimiter.Trigger('A'));
        Assert.Equal(RateLimitStatus.Limited, entityRateLimiter.Trigger('A'));

        System.Threading.Thread.Sleep(1100);

        Assert.Equal(RateLimitStatus.Accessible, entityRateLimiter.Trigger('A'));
    }
}
