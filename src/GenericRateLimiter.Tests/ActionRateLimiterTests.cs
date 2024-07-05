using GenericRateLimiter.Core;

namespace GenericRateLimiter.Tests;

using System;
using GenericRateLimiter;
using Xunit;

public class ActionRateLimiterTests
{
    [Fact]
    public void Trigger_ShouldDecreaseLimit()
    {
        var rateLimiter = new ActionRateLimiter(2, TimeSpan.FromSeconds(1));

        Assert.False(rateLimiter.Trigger());
        Assert.False(rateLimiter.Trigger());
        Assert.True(rateLimiter.Trigger());
    }

    [Fact]
    public void Trigger_ShouldResetAfterPeriod()
    {
        var rateLimiter = new ActionRateLimiter(1, TimeSpan.FromSeconds(1));

        Assert.False(rateLimiter.Trigger());
        Assert.True(rateLimiter.Trigger());

        Thread.Sleep(1100);

        Assert.False(rateLimiter.Trigger());
    }
}
