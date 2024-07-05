# Generic rate limiter
Simple universal rate limiter. Don't use this for ASP.NET, use built in rate limiter instead.

### Usage
```csharp
builder.Services.AddRateLimiter<string>(x =>
{
    x.AddRateLimiter(1, TimeSpan.FromSeconds(1));
    x.AddRateLimiter(5, TimeSpan.FromSeconds(10));
});
```
Allows 1 request per 1 second and 5 requests in 10 seconds.
```csharp
public class SampleService(IEntityRateLimiter<int> entityRateLimiter) : IHostedService
{
    public async Task Method(CancellationToken cancellationToken)
    {
        // use any unique request identifier
        var rateLimitStatus = entityRateLimiter.Trigger("some request id");
    }
```