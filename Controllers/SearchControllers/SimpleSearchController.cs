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
    public async Task<List<SearchResultDTO>> Post(SimpleSearchArgsDTO argsDto)
    {
        if (string.IsNullOrEmpty(argsDto?.Criterion))
        {
            return new List<SearchResultDTO>();
        }

        var criterion = argsDto?.Criterion.ToLower();

        // Criterion is searched into the title, the author name, the ISBN and the series' name
        /*Func<Book, IQueryable<IGrouping<string, Edition>>>*/
        IQueryable<IGrouping<string, Edition>> editionsGroupingRequest(int bookId) {
            return from edition in _context.Editions
                   join book in _context.Books on edition.BookId equals bookId
                   join series in _context.Series on edition.SeriesId equals series.Id
                   group edition by series.Name into groupedBySeries
                   select groupedBySeries;
        };

        var resultDtos = await (from boo in _context.Books
                          join author in _context.Authors on boo.AuthorId equals author.Id
                          join ed in _context.Editions on boo.Id equals ed.BookId
                          join series in _context.Series on ed.SeriesId equals series.Id into seriesJoin
                          from series in seriesJoin.DefaultIfEmpty() // Retrieves editions even if they have no series
                          where boo.Title == "Stupeur et tremblements"
                              || author.CompleteName == "Hondermarck Olivier"
                              || ed.Isbn == "9782811620943"
                              || series.Name == "Le Monde de Narnia"
                          select new SearchResultDTO(_manager.Mapper.Map<BookResultDTO>(boo)))
                        //  .Include(b => b.Author)
                        //.Include(b => b.Genre)
                        //.Include(b => b.Editions)
                        //.ThenInclude(ed => ed.Format)
                        //.Include(b => b.Editions)
                        //.ThenInclude(ed => ed.Series)
                        //.Include(b => b.Editions)
                        //.ThenInclude(ed => ed.Publisher)
                          .ToListAsync();

        foreach (var dto in resultDtos)
        {
            var editions = await editionsGroupingRequest(dto.BookId).ToListAsync();
            var dico = new Dictionary<string, List<EditionResultDTO>>();

            foreach (var group in editions)
            {
                dico.Add(group.Key, _manager.Mapper.Map<List<EditionResultDTO>>(group.ToList()));
            }

            dto.Editions = dico;
        }

        //var results = await books.Select(book =>
        //{
        //    return new SearchResultDTO(book, _manager.Mapper)
        //    {

        //    };
        //});

        //Include(b => b.Author)
        //                         .Include(b => b.Genre)
        //                         .Include(b => b.Editions)
        //                         .ThenInclude(ed => ed.Format)
        //                         .Include(b => b.Editions)
        //                         .ThenInclude(ed => ed.Series)
        //                         .Include(b => b.Editions)
        //                         .ThenInclude(ed => ed.Publisher)
        //                         .ToListAsync();

        //return _manager.GetSimpleSearchResults(results);

        return resultDtos;
    }
}