using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Managers.SearchManagers;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers.SearchControllers;

/// <summary>
/// API requests for the advanced search
/// which accepts multiple criteria into an object
/// </summary>
[ApiController]
[Route("/search/advanced")]
public class AdvancedSearchController : ControllerBase
{
    /// <summary>
    /// Logger for the AdvancedSearchController
    /// </summary>
    protected readonly ILogger<AdvancedSearchController> _logger;

    /// <summary>
    /// Methods for preparing the data sent by the AdvancedSearchController
    /// </summary>
    public readonly SimpleSearchManager _manager;

    /// <summary>
    /// Constructor of the AdvancedSearchController class
    /// </summary>
    /// <param name="logger">Given Logger</param>
    /// <param name="manager">Given SimpleSearchManager with data process methods</param>
    public AdvancedSearchController(
        ILogger<AdvancedSearchController> logger, 
        SimpleSearchManager manager
    )
    {
        _logger = logger;
        _manager = manager;
    }

    /// <summary>
    /// Post CRUD request for the advanced search.
    /// </summary>
    /// <param name="criteria">Object representing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    [HttpPost]
    public async Task<List<SearchResultDTO>> Post(AdvancedSearchArgsDTO criteria)
    {
        return await Task.Run(async() => {
            var test = criteria?.Title;
            return new List<SearchResultDTO>();
        });
    }
}