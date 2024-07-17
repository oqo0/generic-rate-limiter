namespace GenericRateLimiter.Core;

public interface IRateLimiter
{
    long GetLimit();
    TimeSpan GetPeriod();
    TimeSpan GetBanPeriod();
    bool Trigger();
}