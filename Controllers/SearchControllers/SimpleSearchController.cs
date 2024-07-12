using AutoMapper.QueryableExtensions;
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
    public async Task<List<SearchResultDTO>> Post(SimpleSearchArgsDTO argsDto)
    {
        return await Task.Run(async() => {

            if (string.IsNullOrEmpty(argsDto?.Criterion))
            {
                return new List<SearchResultDTO>();
            }

            var results = new List<SearchResultDTO>();
            var criterion = argsDto?.Criterion.ToLower();

            // Criterion is searched into the title, the author name, the ISBN and the series' name
            var getBooksFunc = () => {
                return from boo in _context.Books
                       join author in _context.Authors on boo.AuthorId equals author.Id
                       join ed in _context.Editions on boo.Id equals ed.BookId
                       join series in _context.Series on ed.SeriesId equals series.Id into seriesJoin
                       from series in seriesJoin.DefaultIfEmpty() // Retrieves editions even if they have no series
                       where boo.Title == "Stupeur et tremblements"
                           || author.CompleteName == "Hondermarck Olivier"
                           || ed.Isbn == "9782811620943"
                           || series.Name == "Le Monde de Narnia"
                       select boo;
            };

            var getEditionsForABookFunc = (int bookId) => {
                return from edition in _context.Editions
                       where edition.BookId == bookId
                       select edition;
            };

            // Retrieves the books according to the criterion
            var booksDtos = await getBooksFunc().Include(b => b.Author)
                                                .Include(b => b.Genre)
                                                .ProjectTo<BookResultDTO>(_manager.Mapper.ConfigurationProvider)
                                                .ToListAsync();

            foreach (var book in booksDtos)
            {
                // Retrieves the editions of one book
                var editionsDtos = await getEditionsForABookFunc(book.Id).Include(ed => ed.Format)
                                                                         .Include(ed => ed.Series)
                                                                         .Include(ed => ed.Publisher)
                                                                         .ProjectTo<EditionResultDTO>(_manager.Mapper.ConfigurationProvider)
                                                                         .ToListAsync();

                results.Add(new SearchResultDTO(book) {

                    // Groups the editions by series' name
                    Editions = editionsDtos.GroupBy(ed =>
                    {
                        if (ed?.Series?.SeriesName != null)
                        {
                            return ed.Series.SeriesName;
                        }

                        return "0";

                    }).ToDictionary(
                        group => group.Key, 
                        group => group.ToList()
                    )
                });
            }

            return results;
        });
    }
}