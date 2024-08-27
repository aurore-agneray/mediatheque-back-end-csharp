namespace mediatheque_back_csharp.Middlewares;

/// <summary>
/// Configures the prefix used by all routes of the API
/// </summary>
public class GlobalRoutePrefixMiddleware
{
    /// <summary>
    /// Delegate used for launching the current request
    /// after the prefix has been inserted into it
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// The prefix we want to use
    /// </summary>
    private readonly string _routePrefix;

    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="next">Delegate used for launching the current request</param>
    /// <param name="routePrefix">Prefix</param>
    public GlobalRoutePrefixMiddleware(RequestDelegate next, string routePrefix)
    {
        _next = next;
        _routePrefix = routePrefix;
    }

    /// <summary>
    /// Launches the middleware treatments
    /// </summary>
    /// <param name="context">HTTP Context with the current request</param>
    /// <returns>A task object which represents a void async process</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Wait for the request to begin with the prefix
        if (!context.Request.Path.StartsWithSegments(_routePrefix))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Not Found");
            return;
        }
        await _next(context);
    }
}