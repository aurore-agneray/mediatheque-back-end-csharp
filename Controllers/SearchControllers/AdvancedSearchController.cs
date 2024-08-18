using mediatheque_back_csharp.Managers.SearchManagers;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers.SearchControllers;

/// <summary>
/// API requests for the advanced search
/// which accepts multiple criteria into an object
/// </summary>
[ApiController]
[Route("/search/advanced")]
public class AdvancedSearchController : SearchController
{
    /// <summary>
    /// Constructor of the AdvancedSearchController class
    /// </summary>
    /// <param name="logger">Given Logger</param>
    /// <param name="manager">Given SimpleSearchManager with data process methods</param>
    public AdvancedSearchController(ILogger<AdvancedSearchController> logger, AdvancedSearchManager manager) : base(logger, manager)
    {
    }
}