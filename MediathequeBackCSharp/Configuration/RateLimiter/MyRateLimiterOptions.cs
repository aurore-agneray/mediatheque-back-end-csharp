using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using MediathequeBackCSharp.Texts;
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
    private static TokenBucketPolicyOptions GetPolicyOptions(WebApplicationBuilder appBuilder, string policyName)
    {
        string configurationPath = $"{CONFIGURATION_RATE_LIMITER_SECTION}:{policyName}";

        var options = appBuilder.GetAppSettingsNode<TokenBucketPolicyOptions>(configurationPath);

        return options ?? throw new Exception(InternalErrorTexts.ERROR_MISSING_RATE_LIMITER_CONFIG);
    }

    /// <summary>
    /// This global policy concerns each user !!
    /// Applied to all requests without adding the mandatory attribute into the concerned controllers
    /// Source : https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-8.0#rate-limiter-samples
    /// IS NOT APPLIED IN A LOCAL CONTEXT !!!!!
    /// </summary>
    private static PartitionedRateLimiter<HttpContext> GetGlobalLimiter(WebApplicationBuilder appBuilder, out TokenBucketPolicyOptions globalOptions)
    {
        globalOptions = GetPolicyOptions(appBuilder, GLOBAL_POLICY_NAME);
        /* The options are stored into another value in order to be used into the lambda expression below.
         * Without doing this, the code analyser displays an error because an "out" variable can't be used within a lambda exp */
        var optionsForLambdaExp = globalOptions;

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
                        TokenLimit = optionsForLambdaExp.TokenLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = optionsForLambdaExp.QueueLimit,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(optionsForLambdaExp.ReplenishmentPeriod),
                        TokensPerPeriod = optionsForLambdaExp.TokensPerPeriod,
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
    private static Action<TokenBucketRateLimiterOptions> GetSecondPolicyOptionsAction(WebApplicationBuilder appBuilder, out TokenBucketPolicyOptions policyOptions)
    {
        policyOptions = GetPolicyOptions(appBuilder, SECOND_POLICY_NAME);
        /* The options are stored into another value in order to be used into the lambda expression below.
         * Without doing this, the code analyser displays an error because an "out" variable can't be used within a lambda exp */
        var optionsForLambdaExp = policyOptions;

        return opt =>
        {
            opt.TokenLimit = optionsForLambdaExp.TokenLimit;
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.QueueLimit = optionsForLambdaExp.QueueLimit;
            opt.ReplenishmentPeriod = TimeSpan.FromSeconds(optionsForLambdaExp.ReplenishmentPeriod);
            opt.TokensPerPeriod = optionsForLambdaExp.TokensPerPeriod;
            opt.AutoReplenishment = true;
        };
    }

    /// <summary>
    /// Prepares the response sent to the client when the threshold is reached
    /// </summary>
    /// <param name="rejectionMessage">The error message that is sent to the user</param>
    private static Func<OnRejectedContext, CancellationToken, ValueTask> GetOnRejected(string rejectionMessage)
    {
        return async (context, cancellationToken) =>
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

            // This option is mandatory if we want the response to return an object instead of a string !
            context.HttpContext.Response.ContentType = "application/json";

            var response = new { Message = rejectionMessage };

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
        ITextsManager? textsManager = appBuilder.GetService<ITextsManager>() 
                                      ?? throw new Exception(InternalErrorTexts.ERROR_TEXT_MANAGER_RETRIEVAL);

        var globalLimiter = GetGlobalLimiter(appBuilder, out TokenBucketPolicyOptions globalLimiterOptions);
        var secondLimiterOptionsAction = GetSecondPolicyOptionsAction(appBuilder, out TokenBucketPolicyOptions secondLimiterOptions);

        // Defines the number of seconds the user must wait to initiate a new request if he reaches a threshold
        int maxTimeForWaiting = Math.Max(globalLimiterOptions.ReplenishmentPeriod, secondLimiterOptions.ReplenishmentPeriod);

        string rejectionMessage = string.Format(
            textsManager.GetText(TextsKeys.ERROR_RATE_LIMIT_REACHED),
            maxTimeForWaiting
        );

        return options =>
        {
            options.GlobalLimiter = globalLimiter;
            options.AddTokenBucketLimiter(SECOND_POLICY_NAME, secondLimiterOptionsAction);
            options.OnRejected = GetOnRejected(rejectionMessage);
        };
    }
}