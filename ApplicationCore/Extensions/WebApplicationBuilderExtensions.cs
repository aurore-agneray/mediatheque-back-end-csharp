using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace ApplicationCore.Extensions;

/// <summary>
/// Extension methods dedicated to WebApplicationBuilder objects
/// </summary>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Retrieves a unique string value from the appsettings file
    /// attached to the executed web project
    /// </summary>
    /// <param name="appBuilder">Web application builder</param>
    /// <param name="path">Path defined into the appsettings file for the wanted value</param>
    /// <returns>A string value if the path exists, or an empty value</returns>
    public static string GetAppSettingsValue(this WebApplicationBuilder appBuilder, string path)
    {
        return appBuilder.Configuration.GetValue<string>(path) ?? string.Empty;
    }

    /// <summary>
    /// Retrieves a node from the appsettings file
    /// attached to the executed web project. It will be converted
    /// into a specific object
    /// </summary>
    /// <typeparam name="T">Type of the object that will represent the wanted node</typeparam>
    /// <param name="appBuilder">Web application builder</param>
    /// <param name="path">Path defined into the appsettings file for the wanted node</param>
    /// <returns>An object if the path exists, or null</returns>
    public static T? GetAppSettingsNode<T>(this WebApplicationBuilder appBuilder, string path)
        where T : class
    {
        return appBuilder.Configuration.GetSection(path).Get<T>();
    }
}