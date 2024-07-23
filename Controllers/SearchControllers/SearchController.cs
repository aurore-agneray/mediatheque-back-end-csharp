using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Managers.SearchManagers;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers.SearchControllers;

/// <summary>
/// API's requests for the book search
/// </summary>
[ApiController]
[Route("")]
public class SearchController : ControllerBase
{
    /// <summary>
    /// Logger for the SearchController
    /// </summary>
    protected readonly ILogger<SearchController> _logger;

    /// <summary>
    /// Methods for preparing the data sent by the SearchController
    /// </summary>
    public readonly SearchManager _manager;

    /// <summary>
    /// Constructor of the SearchController class
    /// </summary>
    /// <param name="logger">Given Logger</param>
    /// <param name="manager">Given SimpleSearchManager with data process methods</param>
    public SearchController(ILogger<SearchController> logger, SearchManager manager)
    {
        _logger = logger;
        _manager = manager;
    }

    /// <summary>
    /// Post CRUD request for the research.
    /// </summary>
    /// <param name="criteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    [HttpPost]
    public async Task<List<SearchResultDTO>> Post(SearchArgsDTO criteria)
    {
        return await _manager.SearchForResults(criteria);
    }
}