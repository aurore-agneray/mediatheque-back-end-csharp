using AutoMapper;
using AutoMapper.QueryableExtensions;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Extensions;
using mediatheque_back_csharp.Pocos;
using Microsoft.EntityFrameworkCore;
using static System.Linq.Queryable;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the SimpleSearchController
/// </summary>
public class SimpleSearchManager
{
    /// <summary>
    /// HTTP Context for connecting to the database
    /// </summary>
    private readonly MediathequeDbContext _context;
    
    /// <summary>
    /// Transforms the POCOs into DTOs
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor of the SimpleSearchManager class
    /// </summary>
    /// <param name="context">HTTP Context</param>
    /// <param name="mapper">Given AutoMapper</param>
    public SimpleSearchManager(MediathequeDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="criterion">Title, author name, ISBN or series' name</param>
    /// <returns>A IQueryable<Book> object</returns>
    private IQueryable<Book> GetOrderedBooksRequest(string criterion) {

        return (
            from boo in _context.Books
            join author in _context.Authors on boo.AuthorId equals author.Id
            join ed in _context.Editions on boo.Id equals ed.BookId
            join series in _context.Series on ed.SeriesId equals series.Id into seriesJoin
            from series in seriesJoin.DefaultIfEmpty() // Retrieves editions even if they have no series
            where (boo.Title != null && boo.Title.Contains(criterion))
                || (author.CompleteName != null && author.CompleteName.Contains(criterion))
                || (ed.Isbn != null && ed.Isbn.Contains(criterion))
                || (series.Name != null && series.Name.Contains(criterion))
            select boo
        )
        .Distinct()
        .OrderBy(b => b.Title);
    }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the editions from the database
    /// </summary>
    /// <param name="bookId">ID of the concerned book</param>
    /// <returns>A IQueryable<Edition> object</returns>
    private IQueryable<Edition> GetEditionsForABookRequest(int bookId) {
        return from edition in _context.Editions
               where edition.BookId == bookId
               select edition;
    }

    /// <summary>
    /// Order the given list by volumes' alphanumerical ascending order
    /// </summary>
    /// <param name="editions">List of EditionResultDTO objects</param>
    /// <returns>Ordered list of EditionResultDTO objects</returns>
    private List<EditionResultDTO> OrderEditionsByVolume(IEnumerable<EditionResultDTO> editions)
    {
        if (editions == null) {
            return new List<EditionResultDTO>();
        }
        
        return editions.OrderBy(item => item.Volume.ExtractPrefix())
                       .ThenBy(item => item.Volume.ExtractNumber())
                       .ToList();
    }

    /// <summary>
    /// Retrieves the results from the database
    /// </summary>
    /// <param name="criterion">Searched into the title, the author name, the ISBN and the series' name</param>
    /// <returns>List of SearchResultDTOs (BookId + Book object + Editions grouped by series' name)</returns>
    public async Task<IEnumerable<SearchResultDTO>> GetSimpleSearchResults(string criterion)
    {
        if (string.IsNullOrEmpty(criterion))
        {
            return new List<SearchResultDTO>();
        }
            
        var results = new List<SearchResultDTO>();
    
        // Retrieves the books according to the criterion
        // Criterion is searched into the title, the author name, the ISBN and the series' name
        var booksDtos = await GetOrderedBooksRequest(criterion).Include(b => b.Author)
                                                        .Include(b => b.Genre)
                                                        .ProjectTo<BookResultDTO>(_mapper.ConfigurationProvider)
                                                        .ToListAsync();

        foreach (var book in booksDtos)
        {
            // Retrieves the editions of one book
            var editionsDtos = await GetEditionsForABookRequest(book.Id).Include(ed => ed.Format)
                                                                        .Include(ed => ed.Series)
                                                                        .Include(ed => ed.Publisher)
                                                                        .ProjectTo<EditionResultDTO>(_mapper.ConfigurationProvider)
                                                                        .ToListAsync();

            results.Add(new SearchResultDTO(book) {
                // Groups the editions by series' name
                Editions = OrderEditionsByVolume(editionsDtos).GroupElementsBySeriesName()
            });
        }

        return results;
    }
}