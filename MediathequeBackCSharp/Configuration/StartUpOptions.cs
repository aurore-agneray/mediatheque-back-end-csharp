using MediathequeBackCSharp.Configuration.Swagger;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MediathequeBackCSharp.Configuration;

/// <summary>
/// Defines the options for the services declared into the Program.cs file
/// </summary>
public static class StartUpOptions
{
    /// <summary>
    /// Generates the general options for the Swagger service
    /// </summary>
    /// <param name="assemblyVersion">Version number of our web assembly</param>
    /// <param name="routePrefix">Prefix used for all requests of the app</param>
    /// <returns>An Action object</returns>
    public static Action<SwaggerGenOptions> GetSwaggerGenOptions(string assemblyVersion, string routePrefix)
    {
        var configurator = new SwaggerOptionsConfigurator();

        return options =>
        {
            configurator.Configure(assemblyVersion, options, routePrefix);
        };
    }

    /// <summary>
    /// Generates the general options for the Cors service
    /// (for instance the accepted front domains)
    /// </summary>
    /// <param name="frontEndDomains">Allowed front-end domains for calling the API,
    /// separated by ;</param>
    /// <returns>An Action object</returns>
    public static Action<CorsOptions> GetCorsOptions(string frontEndDomains)
    {
        if (string.IsNullOrEmpty(frontEndDomains))
        {
            throw new ArgumentException("Some settings are missing for configuring CORS policy");
        }

        string[] frontDomains = frontEndDomains.Split(';');

        return options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins(frontDomains.Length > 0 ? frontDomains : ["http://localhost:5173"])
                          .WithHeaders("Content-type")
                          .WithMethods("GET", "POST");
                });
        };
    }
}