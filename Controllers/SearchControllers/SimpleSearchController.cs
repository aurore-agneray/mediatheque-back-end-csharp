using mediatheque_back_csharp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mediatheque_back_csharp.Controllers.SearchControllers;

/// <summary>
/// API requests for the simple search
/// which accepts a unique criterion (Author name, Book title, ISBN or series name)
/// </summary>
[ApiController]
[Route("[controller]")]
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
    /// Constructor of the SimpleSearchController class
    /// </summary>
    /// <param name="context">Given database context</param>
    /// <param name="logger">Given Logger</param>
    public SimpleSearchController(MediathequeDbContext context, ILogger<SimpleSearchController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get CRUD request for the simple search.
    /// </summary>
    /// <param name="criterion">Words representing the search criterion</param>
    /// <returns>List of some IIdentified objects of the database</returns>
    [HttpGet]
    public async Task<IEnumerable<IIdentified>> Get(string? criterion)
    {
        if (criterion == null || criterion == string.Empty) 
        {
            return new List<IIdentified>();
        }

        criterion = criterion.ToLower();

        var books = _context.Books
                            .Where(book => (
                                                book.Title != null
                                                && book.Title.ToLower().Contains(criterion)
                                           )
                                           || (
                                                book.Author != null
                                                && book.Author.CompleteName != null
                                                && book.Author.CompleteName.ToLower().Contains(criterion)
                                           )
                                           || (
                                                book.Editions != null
                                                && book.Editions.Any(edition => edition.Isbn == criterion 
                                                   || (edition.Series != null && edition.Series.Name != null && edition.Series.Name.ToLower().Contains(criterion)))
                                           )
                                        );

        return await books.ToListAsync();
    }
}