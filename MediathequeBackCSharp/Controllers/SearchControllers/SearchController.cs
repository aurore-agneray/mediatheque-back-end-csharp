using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using MediathequeBackCSharp.Classes;
using MediathequeBackCSharp.Texts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;

namespace MediathequeBackCSharp.Controllers.SearchControllers;

/// <summary>
/// API's requests for the book search
/// </summary>
/// <remarks>
/// Constructor of the SearchController class
/// </remarks>
/// <param name="logger">Given Logger</param>
/// <param name="manager">Given SimpleSearchManager with data process methods</param>
[ApiController]
[Route("/search")]
[EnableRateLimiting("fixedLimiter")]
public abstract class SearchController(ILogger<SearchController> logger, ISearchManager<IAllSearchServices> manager) : ControllerBase
{
    /// <summary>
    /// Logger for the SearchController
    /// </summary>
    protected readonly ILogger<SearchController> _logger = logger;

    /// <summary>
    /// Methods for preparing the data sent by the SearchController
    /// </summary>
    public readonly ISearchManager<IAllSearchServices> _manager = manager;

    /// <summary>
    /// Execute the POST request common instructions for all kind of SearchControllers
    /// </summary>
    /// <param name="searchCriteria">Criteria for searching the books</param>
    /// <param name="searchType">Type of search (simple, advanced, MySQL, BnF, etc)</param>
    /// <returns>Returns the result for the client</returns>
    /// <response code="500">If the wanted type search has not been implemented</response>
    protected async Task<IActionResult> ExecutePostRequest(SearchDTO searchCriteria, SearchTypeEnum searchType)
    {
        if (_manager == null || searchCriteria == null)
        {
            return NotFound();
        }

        if (searchType == SearchTypeEnum.NotImplemented)
        {
            return new ErrorObject(
                HttpStatusCode.InternalServerError, 
                _manager.TextsManager.GetString(TextsKeys.ERROR_NO_IMPLEMENTED_SEARCH_TYPE) ?? string.Empty
            );
        }

        try
        {
            var results = await _manager.SearchForResults(searchCriteria, searchType);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return new ErrorObject(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}