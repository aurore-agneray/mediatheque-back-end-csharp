namespace MediathequeBackCSharp.Middlewares;

/// <summary>
/// Configures the prefix used by all routes of the API
/// </summary>
/// <remarks>
/// Main constructor
/// </remarks>
/// <param name="next">Delegate used for launching the current request</param>
/// <param name="routePrefix">Prefix</param>
public class GlobalRoutePrefixMiddleware(RequestDelegate next, string routePrefix)
{
    /// <summary>
    /// Launches the middleware treatments
    /// </summary>
    /// <param name="context">HTTP Context with the current request</param>
    /// <returns>A task object which represents a void async process</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Wait for the request to begin with the prefix
        if (!context.Request.Path.StartsWithSegments(routePrefix))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Not Found");
            return;
        }
        await next(context);
    }
}