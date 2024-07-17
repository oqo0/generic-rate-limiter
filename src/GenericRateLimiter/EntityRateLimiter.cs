using GenericRateLimiter.Core;
using GenericRateLimiter.Core.RateLimiters;
using GenericRateLimiter.Core.WasteCleaners;

namespace GenericRateLimiter;

public class EntityRateLimiter<TId> : IEntityRateLimiter<TId>
    where TId : notnull
{
    private readonly RateLimiterRepository<TId> _rateLimiterRepository;
    private readonly IEnumerable<IRateLimiter> _rateLimiters;

    public EntityRateLimiter(
        IEnumerable<ActionRateLimiter> rateLimiters,
        WasteCleanerSettings wasteCleanerSettings)
    {
        _rateLimiters = rateLimiters;
        _rateLimiterRepository = new RateLimiterRepository<TId>(wasteCleanerSettings);
    }
    
    public RateLimitStatus Trigger(TId id)
    {
        if (GetRateLimitStatus(id, out var rateLimitStatus))
            return rateLimitStatus;

        var newCompositeRateLimiter = GetNewCompositeRateLimiter(id);

        return newCompositeRateLimiter.Trigger()
            ? RateLimitStatus.Limited
            : RateLimitStatus.Accessible;
    }

    private bool GetRateLimitStatus(TId id, out RateLimitStatus rateLimitStatus)
    {
        bool compositeRateLimiterFound = _rateLimiterRepository.TryGet(id, out var compositeRateLimiter)
                                         && compositeRateLimiter is not null;
        if (compositeRateLimiterFound)
        {
            rateLimitStatus = compositeRateLimiter!.Trigger()
                ? RateLimitStatus.Limited
                : RateLimitStatus.Accessible;
            return true;
        }

        rateLimitStatus = RateLimitStatus.Accessible;
        return false;
    }
    
    private RateLimiterComposite GetNewCompositeRateLimiter(TId id)
    {
        var actionRateLimiters = _rateLimiters.Select(rl =>
            new ActionRateLimiter(rl.GetLimit(), rl.GetPeriod(), rl.GetBanPeriod()))
            .ToList();
        
        var newCompositeRateLimiter = new RateLimiterComposite(actionRateLimiters);
        _rateLimiterRepository.AddOrUpdate(id, newCompositeRateLimiter);
        return newCompositeRateLimiter;
    }
}