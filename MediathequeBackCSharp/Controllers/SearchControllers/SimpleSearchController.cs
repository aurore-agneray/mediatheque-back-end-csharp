using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Enums;
using MediathequeBackCSharp.Managers.SearchManagers;
using Microsoft.AspNetCore.Mvc;

namespace MediathequeBackCSharp.Controllers.SearchControllers;

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
    /// Processes the simple search with a unique criterion
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects OR an error</returns>
    /// <remarks>
    /// The available search types are :
    /// - from the MySQL database
    /// - from the BnF API
    /// 
    /// The available values for the BnF's notices quantity are 20, 100, 200, 500 and 1000.
    /// With other values, the API will return an error.
    /// </remarks>
    /// <response code="404">If the search criteria object is null</response>
    /// <response code="500">If an error occurred into the process, with an explicit information message</response>
    [HttpPost]
    public async Task<IActionResult> Post(SimpleSearchDTO searchCriteria)
    {
        if (searchCriteria == null)
        {
            return NotFound();
        }

        SearchTypeEnum searchType = searchCriteria.UseBnfApi
                                    ? SearchTypeEnum.BnfAPISimple
                                    : SearchTypeEnum.MySQLSimple;

        return await ExecutePostRequest(searchCriteria, searchType);
    }
}