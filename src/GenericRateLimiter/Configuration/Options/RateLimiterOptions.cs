using GenericRateLimiter.Core;

namespace GenericRateLimiter.Configuration.Options;

public class RateLimiterOptions()
{
    private readonly List<ActionRateLimiter> _actionRateLimiters = [];

    public RateLimiterOptions AddRateLimiter(int limit, TimeSpan period)
    {
       _actionRateLimiters.Add(new ActionRateLimiter(limit, period));
       return this;
    }

    public List<ActionRateLimiter> GetActionRateLimiters()
    {
        return _actionRateLimiters;
    }
}