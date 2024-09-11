using ApplicationCore.Interfaces.Databases;
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
    /// <param name="settings">Settings given for the concerned database</param>
    /// <returns>An Action object</returns>
    public static Action<CorsOptions> GetCorsOptions(IDatabaseSettings settings)
    {
        if (settings == null)
        {
            return options => { };
        }

        var frontDomainsStr = settings?.FrontEndDomains;
        string[] frontDomains = null;

        if (frontDomainsStr != null)
        {
            frontDomains = frontDomainsStr.Split(';');
        }

        return options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins(frontDomains ?? ["http://localhost:5173"])
                          .WithHeaders("Content-type")
                          .WithMethods("GET", "POST");
                });
        };
    }
}