namespace GenericRateLimiter;

public interface IEntityRateLimiter<in TId>
    where TId : notnull
{
    public RateLimitStatus Trigger(TId id);
}