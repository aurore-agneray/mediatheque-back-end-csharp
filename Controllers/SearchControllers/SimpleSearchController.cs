using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Managers.SearchManagers;
using mediatheque_back_csharp.Services;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers.SearchControllers;

/// <summary>
/// API requests for the simple search
/// which accepts a unique criterion (Author name, Book title, ISBN or series name)
/// </summary>
[ApiController]
[Route("/search/simple")]
public class SimpleSearchController : ControllerBase
{
    /// <summary>
    /// Logger for the SimpleSearchController
    /// </summary>
    protected readonly ILogger<SimpleSearchController> _logger;

    /// <summary>
    /// Methods for preparing the data sent by the SimpleSearchController
    /// </summary>
    public readonly SimpleSearchManager _manager;

    /// <summary>
    /// Constructor of the SimpleSearchController class
    /// </summary>
    /// <param name="logger">Given Logger</param>
    /// <param name="manager">Given SimpleSearchManager with data process methods</param>
    public SimpleSearchController(
        ILogger<SimpleSearchController> logger, 
        SimpleSearchManager manager
    )
    {
        _logger = logger;
        _manager = manager;
    }

    /// <summary>
    /// Post CRUD request for the simple search.
    /// </summary>
    /// <param name="argsDto">Contains the parameters sent by the browser
    /// ("criterion", a boolean "apiBnf" and the number of studied BnF's notices)</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    [HttpPost]
    public async Task<List<SearchResultDTO>> Post(SimpleSearchArgsDTO argsDto)
    {
        return await Task.Run(async() => {

            if (argsDto == null || string.IsNullOrEmpty(argsDto?.Criterion))
            {
                return new List<SearchResultDTO>();
            }

            IEnumerable<SearchResultDTO> results;

            var criterion = argsDto?.Criterion.ToLower();

            // Criterion is searched into the title, the author name, the ISBN and the series' name
            if (argsDto.ApiBnf)
            {
                var bnfService = new BnfSearchService(false);
                results = await bnfService.GetResults(criterion, argsDto.ApiBnfNoticesQty);
            }
            else
            {
                results = await _manager.GetSimpleSearchResults(criterion);
            }

            return results.ToList();
        });
    }
}