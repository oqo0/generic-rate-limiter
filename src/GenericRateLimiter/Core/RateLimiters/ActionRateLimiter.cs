namespace GenericRateLimiter.Core.RateLimiters;

public class ActionRateLimiter(long limit, TimeSpan period) : IRateLimiter
{
    public long Limit { get; } = limit;
    public TimeSpan Period { get; } = period;

    private long _currentLimit = limit;
    private DateTime _lastResetTime = DateTime.UtcNow;

    public long GetLimit()
    {
        return Limit;
    }

    public TimeSpan GetPeriod()
    {
        return Period;
    }

    /// <summary>
    /// Triggers the rate limiter.
    /// </summary>
    /// <returns>A boolean indicating whether the action is limited.</returns>
    public bool Trigger()
    {
        ResetLimitIfPeriodElapsed();

        if (_currentLimit <= 0)
            return true;
        
        _currentLimit--;
        return false;
    }

    /// <summary>
    /// Resets the current limit if the period has elapsed.
    /// </summary>
    private void ResetLimitIfPeriodElapsed()
    {
        if (DateTime.UtcNow - _lastResetTime <= Period)
            return;
        
        _currentLimit = Limit;
        _lastResetTime = DateTime.UtcNow;
    }
}