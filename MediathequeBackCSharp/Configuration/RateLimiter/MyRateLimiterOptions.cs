using Microsoft.AspNetCore.RateLimiting;
using System.Net;
using System.Threading.RateLimiting;

namespace MediathequeBackCSharp.Configuration.RateLimiter;

/// <summary>
/// Generates the rate limiters options that will prevent the API
/// to be used infinitely
/// </summary>
internal static class MyRateLimiterOptions
{
    /// <summary>
    /// Constant used into the appsettings file
    /// </summary>
    private const string CONFIGURATION_RATE_LIMITER_SECTION = "RateLimiter";

    /// <summary>
    /// Constant used into the appsettings file
    /// </summary>
    private const string GLOBAL_POLICY_NAME = "Global";

    /// <summary>
    /// Constant used into the appsettings file
    /// </summary>
    private const string SECOND_POLICY_NAME = "SecondPolicy";

    /// <summary>
    /// Retrieves the options of the concerned policy from the appsettings file
    /// </summary>
    /// <exception cref="Exception">When the options aren't into the appsettings file</exception>
    //private static RateLimiterPolicyOptions GetPolicyOptions(WebApplicationBuilder appBuilder, string policyName)
    //{
    //    var policyOptions = new RateLimiterPolicyOptions();
    //    string configurationPath = $"{CONFIGURATION_RATE_LIMITER_SECTION}:{policyName}";

    //    if (appBuilder.Configuration.GetSection(configurationPath) != null)
    //    {
    //        appBuilder.Configuration.GetSection(configurationPath).Bind(policyOptions);
    //        return policyOptions;
    //    }
    //    else
    //    {
    //        throw new Exception("options used for defining the global policy limiter are missing !");
    //    }
    //}

    /// <summary>
    /// This global policy concerns each user !!
    /// Applied to all requests without adding the mandatory attribute into the concerned controllers
    /// Source : https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-8.0#rate-limiter-samples
    /// IS NOT APPLIED IN A LOCAL CONTEXT !!!!!
    /// </summary>
    private static PartitionedRateLimiter<HttpContext> GetGlobalLimiter(WebApplicationBuilder appBuilder)
    {
        var globalOptions = new TokenBucketPolicyOptions
        {
            TokenLimit = 20,
            QueueLimit = 3,
            ReplenishmentPeriod = 60,
            TokensPerPeriod = 30
        }; //GetPolicyOptions(appBuilder, GLOBAL_POLICY_NAME);

        return PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
        {
            IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;

            if (!IPAddress.IsLoopback(remoteIpAddress!))
            {
                return RateLimitPartition.GetTokenBucketLimiter
                (
                    remoteIpAddress!,
                    _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = globalOptions.TokenLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = globalOptions.QueueLimit,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(globalOptions.ReplenishmentPeriod),
                        TokensPerPeriod = globalOptions.TokensPerPeriod,
                        AutoReplenishment = true
                    }
                );
            }

            return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
        });
    }

    /// <summary>
    /// This global policy concerns ALL user as a whole !!
    /// The attribute [EnableRateLimiting] has to be added into the controllers
    /// </summary>
    private static Action<TokenBucketRateLimiterOptions> GetSecondPolicyOptions(WebApplicationBuilder appBuilder)
    {
        var policyOptions = new TokenBucketPolicyOptions
        {
            TokenLimit = 500,
            QueueLimit = 50,
            ReplenishmentPeriod = 60,
            TokensPerPeriod = 30
        }; //GetPolicyOptions(appBuilder, SECOND_POLICY_NAME);

        return opt =>
        {
            opt.TokenLimit = policyOptions.TokenLimit;
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.QueueLimit = policyOptions.QueueLimit;
            opt.ReplenishmentPeriod = TimeSpan.FromSeconds(policyOptions.ReplenishmentPeriod);
            opt.TokensPerPeriod = policyOptions.TokensPerPeriod;
            opt.AutoReplenishment = true;
        };
    }

    /// <summary>
    /// Prepares the response sent to the client when the threshold is reached
    /// </summary>
    private static Func<OnRejectedContext, CancellationToken, ValueTask> GetOnRejected()
    {
        return async (context, cancellationToken) =>
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

            // This option is mandatory if we want the response to return an object instead of a string !
            context.HttpContext.Response.ContentType = "application/json";

            var message = "veuillez attendre 30 secondes avant de lancer une nouvelle recherche";
            var response = new { Message = message };

            await context.HttpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken
            );
        };
    }

    /// <summary>
    /// Generates the rate limiters options that will prevent the API
    /// to be used infinitely
    /// </summary>
    /// <param name="appBuilder">The app builder which permits to read the appsettings file</param>
    internal static Action<RateLimiterOptions> GetOptions(WebApplicationBuilder appBuilder)
    {
        return options =>
        {
            options.GlobalLimiter = GetGlobalLimiter(appBuilder);
            options.AddTokenBucketLimiter(SECOND_POLICY_NAME, GetSecondPolicyOptions(appBuilder));
            options.OnRejected = GetOnRejected();
        };
    }
}