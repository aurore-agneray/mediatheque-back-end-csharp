using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Enums;
using MediathequeBackCSharp.Managers.SearchManagers;
using Microsoft.AspNetCore.Mvc;

namespace MediathequeBackCSharp.Controllers.SearchControllers;

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
    /// <response code="500">If an error occurred into the process, with an explicit information message</response>
    [HttpPost]
    public async Task<IActionResult> Post(AdvancedSearchDTO searchCriteria)
    {
        SearchTypeEnum searchType = SearchTypeEnum.MySQLAdvanced;
        return await ExecutePostRequest(searchCriteria, searchType);
    }
}