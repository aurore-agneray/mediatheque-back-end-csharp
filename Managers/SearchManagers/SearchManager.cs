using AutoMapper;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Pocos;

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
    /// Abstract method that processes the simple or the advanced search
    /// </summary>
    /// <param name="criteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    public abstract Task<List<SearchResultDTO>> SearchForResults(SearchArgsDTO criteria);
}