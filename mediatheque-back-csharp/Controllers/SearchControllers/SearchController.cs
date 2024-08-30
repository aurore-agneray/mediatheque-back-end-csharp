using ApplicationCore;
using ApplicationCore.DTOs.SearchDTOs.CriteriaDTOs;
using ApplicationCore.Texts;
using mediatheque_back_csharp.Classes;
using mediatheque_back_csharp.Managers.SearchManagers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace mediatheque_back_csharp.Controllers.SearchControllers;

/// <summary>
/// API's requests for the book search
/// </summary>
[ApiController]
[Route("/search")]
public abstract class SearchController : ControllerBase
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
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects OR an error</returns>
    [HttpPost]
    public async Task<IActionResult> Post(SearchCriteriaDTO searchCriteria)
    {
        if (_manager == null || searchCriteria == null) {
            return NotFound();
        }

        if (_manager?.TextsManager == null) {
            return new ErrorObject(
                HttpStatusCode.InternalServerError,
                Constants.ERROR_TEXTS_RESOURCES_READING
            );
        }

        if (!_manager.IsDatabaseAvailable())
        {
            var errorMessage = _manager.TextsManager.GetString(TextsKeys.ERROR_DATABASE_CONNECTION) ?? string.Empty;

            return new ErrorObject(
                HttpStatusCode.InternalServerError, 
                errorMessage
            );
        }

        try {
            var results = await _manager.SearchForResults(searchCriteria);
            return Ok(results);
        }
        catch(Exception ex) {
            return new ErrorObject(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}