using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Enums;
using MediathequeBackCSharp.Managers.SearchManagers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MediathequeBackCSharp.Controllers.SearchControllers;

/// <summary>
/// API requests for the advanced search
/// which accepts multiple criteria into an object
/// </summary>
/// <remarks>
/// Constructor of the AdvancedSearchController class
/// </remarks>
/// <param name="logger">Given Logger</param>
/// <param name="manager">Given SimpleSearchManager with data process methods</param>
[ApiController]
[Route("/search/advanced")]
[EnableRateLimiting("fixedLimiter")]
public class AdvancedSearchController(ILogger<AdvancedSearchController> logger, AdvancedSearchManager manager) : SearchController(logger, manager)
{
    /// <summary>
    /// Processes the advanced search with several criteria
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects OR an error</returns>
    /// <remarks>
    /// The available search types are :
    /// - from the MySQL database
    /// - from the BnF API : NOT YET IMPLEMENTED --> the values of "apiBnf" parameters are not used
    /// </remarks>
    /// <response code="404">If the search criteria object is null</response>
    /// <response code="500">If an error occurred into the process, with an explicit information message</response>
    [HttpPost]
    public async Task<IActionResult> Post(AdvancedSearchDTO searchCriteria)
    {
        if (searchCriteria == null)
        {
            return NotFound();
        }

        SearchTypeEnum searchType = searchCriteria.UseBnfApi
                                    ? SearchTypeEnum.NotImplemented
                                    : SearchTypeEnum.MySQLAdvanced;

        return await ExecutePostRequest(searchCriteria, searchType);
    }
}