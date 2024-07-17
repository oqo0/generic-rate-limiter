using GenericRateLimiter.Core;
using GenericRateLimiter.Core.RateLimiters;
using GenericRateLimiter.Core.WasteCleaners;

namespace GenericRateLimiter.Configuration.Options;

public class RateLimiterOptions()
{
    private readonly List<ActionRateLimiter> _actionRateLimiters = [];
    private WasteCleanerSettings _wasteCleanerSettings = GetDefaultWasteCleanerSettings();

    public RateLimiterOptions AddRateLimiter(int limit, TimeSpan period, TimeSpan banTime)
    {
       _actionRateLimiters.Add(new ActionRateLimiter(limit, period, banTime));
       return this;
    }

    public RateLimiterOptions SetWasteCleanerSettings(WasteCleanerSettings wasteCleanerSettings)
    {
        _wasteCleanerSettings = wasteCleanerSettings;
        return this;
    }
    
    public List<ActionRateLimiter> GetActionRateLimiters()
    {
        return _actionRateLimiters;
    }
    
    public WasteCleanerSettings GetWasteCleanerSettings()
    {
        return _wasteCleanerSettings;
    }
    
    private static WasteCleanerSettings GetDefaultWasteCleanerSettings()
    {
        return new WasteCleanerSettings(
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(30));
    }
}