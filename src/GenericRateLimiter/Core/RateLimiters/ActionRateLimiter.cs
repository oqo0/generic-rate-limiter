namespace GenericRateLimiter.Core.RateLimiters;

public class ActionRateLimiter(long limit, TimeSpan period, TimeSpan banPeriod) : IRateLimiter
{
    public long Limit { get; } = limit;
    public TimeSpan Period { get; } = period;
    
    private readonly TimeSpan _banPeriod = banPeriod;
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

    public TimeSpan GetBanPeriod()
    {
        return _banPeriod;
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
        var isBanned = _currentLimit == 0;
        if (isBanned && DateTime.UtcNow - _lastResetTime <= banPeriod)
            return;
        if (DateTime.UtcNow - _lastResetTime <= Period)
            return;
        
        _currentLimit = Limit;
        _lastResetTime = DateTime.UtcNow;
    }
}