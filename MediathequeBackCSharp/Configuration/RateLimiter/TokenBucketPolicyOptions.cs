namespace MediathequeBackCSharp.Configuration.RateLimiter;

/// <summary>
/// Defines the options of a given rate limiter policy of type "Token Bucket"
/// </summary>
public class TokenBucketPolicyOptions
{
    /// <summary>
    /// The maximum quantity of available tokens
    /// </summary>
    public int TokenLimit { get; set; }
    
    /// <summary>
    /// The maximum quantity of requests into the queue
    /// </summary>
    public int QueueLimit { get; set; }

    /// <summary>
    /// The period of time that has to be waited for gettings new tokens
    /// </summary>
    public int ReplenishmentPeriod { get; set; }

    /// <summary>
    /// The number of new got tokens within each replenishment period
    /// </summary>
    public int TokensPerPeriod { get; set; }
}