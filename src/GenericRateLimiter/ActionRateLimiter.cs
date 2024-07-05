namespace GenericRateLimiter;

/// <summary>
/// Represents a rate limiter that limits the number of actions within a specified time period.
/// </summary>
public class ActionRateLimiter
{
    public readonly long Limit;
    public readonly TimeSpan Period;

    private long _currentLimit;
    private DateTime _onsetDateTime;
    private readonly object _lock = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionRateLimiter"/> class.
    /// </summary>
    /// <param name="limit">The maximum number of allowed actions within the time period.</param>
    /// <param name="period">The time period for rate limiting.</param>
    public ActionRateLimiter(long limit, TimeSpan period)
    {
        Limit = limit;
        Period = period;
        _currentLimit = limit;
        _onsetDateTime = DateTime.UtcNow;
    }

    /// <summary>
    /// Attempts to trigger the rate limiter without throwing an error.
    /// </summary>
    /// <returns>True if the action is rate limited, otherwise false.</returns>
    public bool TryTrigger()
    {
        lock (_lock)
        {
            if (IsRateLimited())
            {
                return true;
            }

            _currentLimit--;
            return false;
        }
    }

    private bool IsRateLimited()
    {
        bool isExpired = DateTime.UtcNow - Period > _onsetDateTime;
        if (isExpired)
        {
            _currentLimit = Limit;
            _onsetDateTime = DateTime.UtcNow;
        }

        return _currentLimit <= 0;
    }
}