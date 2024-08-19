using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Managers.SearchManagers;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers.SearchControllers;

/// <summary>
/// API requests for the simple search
/// which accepts a unique criterion (Author name, Book title, ISBN or series name)
/// </summary>
[ApiController]
[Route("/search/simple")]
public class SimpleSearchController : SearchController
{
    /// <summary>
    /// Constructor of the SimpleSearchController class
    /// </summary>
    /// <param name="logger">Given Logger</param>
    /// <param name="manager">Given SimpleSearchManager with data process methods</param>
    public SimpleSearchController(ILogger<SimpleSearchController> logger, SimpleSearchManager manager) : base(logger, manager)
    {
    }
}