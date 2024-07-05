namespace GenericRateLimiter.Core.WasteCleaners;

public interface IWasteCleaner
{
    public void StartPeriodicCleanup(); 
    public Task Cleanup();
}