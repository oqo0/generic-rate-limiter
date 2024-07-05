namespace GenericRateLimiter.Core.WasteCleaners;

internal interface IWasteCleaner
{
    public void StartPeriodicCleanup(); 
    public Task Cleanup();
}