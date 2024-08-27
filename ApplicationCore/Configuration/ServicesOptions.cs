using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApplicationCore.Configuration;

/// <summary>
/// Defines the options for the services declared into the Program.cs file
/// </summary>
public static class ServicesOptions
{
    /// <summary>
    /// Generates the general options for the Swagger service
    /// </summary>
    /// <param name="routePrefix">Prefix used for all requests of the app</param>
    /// <returns>An Action object</returns>
    public static Action<SwaggerGenOptions> GetSwaggerGenOptions(string routePrefix)
    {
        return c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Médiathèque", Version = "v1" });
            c.DocumentFilter<CustomSwaggerFilter>();
            c.AddServer(new OpenApiServer
            {
                Url = routePrefix,
                Description = "Base path for the API"
            });
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