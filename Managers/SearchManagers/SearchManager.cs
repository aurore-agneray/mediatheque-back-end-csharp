using AutoMapper;
using AutoMapper.QueryableExtensions;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Extensions;
using mediatheque_back_csharp.Pocos;
using Microsoft.EntityFrameworkCore;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the SearchController
/// </summary>
public abstract class SearchManager {
    
    /// <summary>
    /// HTTP Context for connecting to the database
    /// </summary>
    protected readonly MediathequeDbContext _context;
    
    /// <summary>
    /// Transforms the POCOs into DTOs
    /// </summary>
    protected readonly IMapper _mapper;

    /// <summary>
    /// Constructor of the SearchManager class
    /// </summary>
    /// <param name="context">HTTP Context</param>
    /// <param name="mapper">Given AutoMapper</param>
    public SearchManager(MediathequeDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="criteria">Criteria sent by the client</param>M
    /// <returns>A IQueryable<Book> object</returns>
    protected abstract IQueryable<Book> GetOrderedBooksRequest(SearchArgsDTO criteria);

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the editions from the database
    /// </summary>
    /// <param name="bookIds">List of the IDs of the concerned books</param>
    /// <returns>A IQueryable<Edition> object</returns>
    protected IQueryable<Edition> GetEditionsForSeveralBooksRequest(int[] bookIds)
    {
        return from edition in _context.Editions
               where bookIds.Contains(edition.BookId)
               select edition;
    }

    /// <summary>
    /// Order the given list by volumes' alphanumerical ascending order
    /// </summary>
    /// <param name="editions">List of EditionResultDTO objects</param>
    /// <returns>Ordered list of EditionResultDTO objects</returns>
    protected List<EditionResultDTO> OrderEditionsByVolume(IEnumerable<EditionResultDTO> editions)
    {
        return editions.OrderBy(item => item?.Volume != null ? item.Volume.ExtractPrefix() : string.Empty)
                       .ThenBy(item => item?.Volume != null ? item.Volume.ExtractNumber() : 0)
                       .ToList();
    }

    /// <summary>
    /// Groups the editions of the given list into a dictionary 
    /// where the keys are the series' names
    /// </summary>
    /// <param name="editions">List of EditionResultDTOs objects</param>
    /// <returns>Returns a dictionary where the keys are the series' names
    /// and the elements are some lists containing editions</returns>
    protected Dictionary<string, List<EditionResultDTO>> GroupEditionsBySeriesName(IEnumerable<EditionResultDTO> editions)
    {
        if (editions == null || editions.Count() == 0)
        {
            return new Dictionary<string, List<EditionResultDTO>>();
        }

        return editions.GroupBy(ed =>
        {
            if (ed?.Series?.SeriesName != null)
            {
                return ed.Series.SeriesName;
            }

            return "0";

        }).ToDictionary(
            group => group.Key,
            group => group.ToList()
        );
    }

    /// <summary>
    /// Processes the search that can be of type "simple" or "advanced".
    /// The difference is defined by the "GetOrderedBooksRequest" call.
    /// </summary>
    /// <param name="criteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    public async Task<List<SearchResultDTO>> SearchForResults(SearchArgsDTO criteria)
    {
        List<BookResultDTO> booksList = new List<BookResultDTO>();
        List<EditionResultDTO> editionsList = new List<EditionResultDTO>();
        List<SearchResultDTO> searchResultsDtos = new List<SearchResultDTO>();

        var booksQuery = GetOrderedBooksRequest(criteria);

        // Completes the first list with the books
        if (booksQuery != null)
        {
            booksList = await booksQuery.Include(b => b.Author)
                                        .Include(b => b.Genre)
                                        .ProjectTo<BookResultDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync();
        }

        // Completes the second list with the editions
        if (booksList != null && booksList.Any())
        {
            editionsList = await this.GetEditionsForSeveralBooksRequest(
                booksList.Select(bDto => bDto.Id).ToArray()
            )
            .Include(ed => ed.Format)
            .Include(ed => ed.Series)
            .Include(ed => ed.Publisher)
            .ProjectTo<EditionResultDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        // Constructs the DTOs
        if (editionsList != null && editionsList.Any())
        {
            searchResultsDtos = booksList.Select(dto =>
                new SearchResultDTO(dto)
            ).ToList();

            searchResultsDtos.ForEach(rDto => {
                rDto.Editions = GroupEditionsBySeriesName(
                    OrderEditionsByVolume(
                        editionsList.Where(ed => ed.BookId == rDto.BookId)
                    )
                );
            });
        }

        return searchResultsDtos;
    }
}