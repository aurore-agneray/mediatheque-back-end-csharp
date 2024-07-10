using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Managers.SearchManagers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    /// Context for connecting to the database
    /// </summary>
    protected readonly MediathequeDbContext _context;

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
    /// <param name="context">Given database context</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="manager">Given SimpleSearchManager with data process methods</param>
    public SimpleSearchController(
        MediathequeDbContext context, 
        ILogger<SimpleSearchController> logger, 
        SimpleSearchManager manager
    )
    {
        _context = context;
        _logger = logger;
        _manager = manager;
    }

    /// <summary>
    /// Post CRUD request for the simple search.
    /// </summary>
    /// <param name="criterion">Words representing the search criterion</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    [HttpPost]
    public async Task<IEnumerable<SearchResultDTO>> Post(SimpleSearchArgsDTO argsDto)
    {
        if (string.IsNullOrEmpty(argsDto?.Criterion))
        {
            return new List<SearchResultDTO>();
        }

        // Criterion is searched into the title, the author name, the ISBN and the series' name
        var books = _context.Books.Where(_manager.GetSearchConditions(argsDto.Criterion));

        var results = await books.Include(b => b.Author)
                                 .Include(b => b.Genre)
                                 .Include(b => b.Editions)
                                 .ThenInclude(ed => ed.Format)
                                 .Include(b => b.Editions)
                                 .ThenInclude(ed => ed.Series)
                                 .Include(b => b.Editions)
                                 .ThenInclude(ed => ed.Publisher)
                                 .ToListAsync();

        return _manager.GetSimpleSearchResults(results);
    }
}