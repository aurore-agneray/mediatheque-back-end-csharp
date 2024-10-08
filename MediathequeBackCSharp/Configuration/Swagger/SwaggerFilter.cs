using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MediathequeBackCSharp.Configuration.Swagger;

/// <summary>
/// Defines which functionalities must appear or not into the Swagger
/// </summary>
public class SwaggerFilter : IDocumentFilter
{
    /// <summary>
    /// Defines which functionalities must appear or not into the Swagger
    /// </summary>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var controllersToHide = new[] { "Search" };

        /* The object ApiDescriptions describes the content of the C# controllers' classes. */
#pragma warning disable CS8602
/* The compiler considers that desc.RelativePath can be null even if I checked for avoiding that */
        var pathsToHide = context.ApiDescriptions
            .Where(desc => controllersToHide.Contains(desc.ActionDescriptor.RouteValues["controller"]) && desc.RelativePath != null)
            .Select(desc => "/" + desc.RelativePath.TrimEnd('/'))
            .ToList();
#pragma warning restore CS8602

        pathsToHide.ForEach(path => swaggerDoc.Paths.Remove(path));
    }
}