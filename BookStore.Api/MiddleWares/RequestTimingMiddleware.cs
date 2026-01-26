namespace BookStore.Api.MiddleWares;

public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    // Constructor Injection works for Singletons (Logger), 
    // but HttpContext is passed into InvokeAsync

    // The Framework calls this automatically
    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Logic BEFORE the controller
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // 2. Pass control to the next middleware (This runs the rest of the app!)
        await next(context);

        // 3. Logic AFTER the controller (Response is ready here)
        stopwatch.Stop();
        
        var elapsed = stopwatch.ElapsedMilliseconds;
        if (elapsed > 500) // Performance warning
        {
            logger.LogWarning("Long running request: {Method} {Path} took {Elapsed}ms", 
                context.Request.Method, context.Request.Path, elapsed);
        }
        else
        {
            logger.LogInformation("running request: {Method} {Path} took {Elapsed}ms", context.Request.Method, context.Request.Path, elapsed);
        }
    }
}