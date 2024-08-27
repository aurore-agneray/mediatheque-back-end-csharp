using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApplicationCore.Configuration;

/// <summary>
/// Defines which functionalities must appear or not into the Swagger
/// </summary>
public class CustomSwaggerFilter : IDocumentFilter
{
    /// <summary>
    /// Defines which functionalities must appear or not into the Swagger
    /// </summary>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var controllersToHide = new[] { "Search" };

        /* The object ApiDescriptions describes the content of the C# controllers' classes. */
        var pathsToHide = context.ApiDescriptions
            .Where(desc => controllersToHide.Contains(desc.ActionDescriptor.RouteValues["controller"]))
            .Select(desc => "/" + desc.RelativePath.TrimEnd('/'))
            .ToList();

        pathsToHide.ForEach(path => swaggerDoc.Paths.Remove(path));
    }
}