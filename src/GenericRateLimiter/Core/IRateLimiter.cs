namespace GenericRateLimiter.Core;

public interface IRateLimiter
{
    long GetLimit();
    TimeSpan GetPeriod();
    bool Trigger();
}