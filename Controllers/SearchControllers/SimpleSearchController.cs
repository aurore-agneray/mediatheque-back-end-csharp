using AutoMapper;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Managers.SearchManagers;
using mediatheque_back_csharp.Pocos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mediatheque_back_csharp.Controllers.SearchControllers;

/// <summary>
/// API requests for the simple search
/// which accepts a unique criterion (Author name, Book title, ISBN or series name)
/// </summary>
[ApiController]
[Route("/api/search/simple")]
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
    /// Transforms the POCOs into DTOs
    /// </summary>
    protected readonly IMapper _mapper;

    /// <summary>
    /// Methods for preparing the data sent by the SimpleSearchController
    /// </summary>
    public readonly SimpleSearchManager _manager;

    /// <summary>
    /// Constructor of the SimpleSearchController class
    /// </summary>
    /// <param name="context">Given database context</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="mapper">Given AutoMapper</param>
    public SimpleSearchController(
        MediathequeDbContext context, 
        ILogger<SimpleSearchController> logger, 
        IMapper mapper, 
        SimpleSearchManager manager
    )
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
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
        if (argsDto?.Criterion == null || argsDto.Criterion == string.Empty) 
        {
            return new List<SearchResultDTO>();
        }

        argsDto.Criterion = argsDto.Criterion.ToLower();

        // Criterion is searched into the title, the author name, the ISBN and the series' name
        var books = _context.Books
                            .Where(book => (
                                                book.Title != null
                                                && book.Title.ToLower().Contains(argsDto.Criterion)
                                           )
                                           || (
                                                book.Author != null
                                                && book.Author.CompleteName != null
                                                && book.Author.CompleteName.ToLower().Contains(argsDto.Criterion)
                                           )
                                           || (
                                                book.Editions != null
                                                && book.Editions.Any(edition => edition.Isbn == argsDto.Criterion
                                                   || (edition.Series != null && edition.Series.Name != null && edition.Series.Name.ToLower().Contains(argsDto.Criterion)))
                                           )
                                        );

        var results = await books.Include(b => b.Author)
                                 .Include(b => b.Genre)
                                 .Include(b => b.Editions)
                                 .ThenInclude(ed => ed.Format)
                                 .Include(b => b.Editions)
                                 .ThenInclude(ed => ed.Series)
                                 .Include(b => b.Editions)
                                 .ThenInclude(ed => ed.Publisher)
                                 .ToListAsync();

        // Maps the books and the editions separately
        // Orders the book by title's ascending alphabetical order
        var bookResultDtos = _mapper.Map<List<Book>, List<BookResultDTO>>(results)
                                    .OrderBy(book => book.Title);

        var editionResultDtos = _mapper.Map<IEnumerable<Edition>, List<EditionResultDTO>>(results.SelectMany(res => res.Editions));

        return bookResultDtos.Select(bookDto =>
        {
            // Binds each book to its editions and orders the editions by volume
            var editions = _manager.OrderEditionsByVolume(
                editionResultDtos.Where(edition => edition.BookId == bookDto.Id)
            );

            return new SearchResultDTO(bookDto)
            {
                // Editions have to be grouped by series' name
                Editions = _manager.GroupEditionsBySeriesName(editions)
            };
        });
    }
}