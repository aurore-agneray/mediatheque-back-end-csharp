namespace MediathequeBackCSharp.Configuration.RateLimiter;

/// <summary>
/// Defines the options of a given rate limiter policy of type "Token Bucket"
/// </summary>
internal class TokenBucketPolicyOptions
{
    /// <summary>
    /// The maximum quantity of available tokens
    /// </summary>
    internal int TokenLimit { get; set; }
    
    /// <summary>
    /// The maximum quantity of requests into the queue
    /// </summary>
    internal int QueueLimit { get; set; }

    /// <summary>
    /// The period of time that has to be waited for gettings new tokens
    /// </summary>
    internal int ReplenishmentPeriod { get; set; }

    /// <summary>
    /// The number of new got tokens within each replenishment period
    /// </summary>
    internal int TokensPerPeriod { get; set; }
}