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
    /// Post CRUD request for the research.
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects OR an error</returns>
    [HttpPost]
    public async Task<IActionResult> Post(AdvancedSearchDTO searchCriteria)
    {
        SearchTypeEnum searchType = SearchTypeEnum.MySQLAdvanced;
        return await ExecutePostRequest(searchCriteria, searchType);
    }
}