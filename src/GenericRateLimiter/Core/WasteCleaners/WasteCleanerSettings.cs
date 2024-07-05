namespace GenericRateLimiter.Core.WasteCleaners;

public record WasteCleanerSettings(TimeSpan CleanupInterval, TimeSpan MaxIdleTime);