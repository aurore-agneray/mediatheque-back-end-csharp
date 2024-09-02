using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Enums;
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

    /// <summary>
    /// Post CRUD request for the research.
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects OR an error</returns>
    [HttpPost]
    public async Task<IActionResult> Post(SimpleSearchDTO searchCriteria)
    {
        SearchTypeEnum searchType = searchCriteria.UseBnfApi
                                    ? SearchTypeEnum.BnfAPISimple
                                    : SearchTypeEnum.MySQLSimple;

        return await ExecutePostRequest(searchCriteria, searchType);
    }
}