using ApplicationCore.AbstractClasses;
using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.DTOs.SearchDTOs.CriteriaDTOs;
using ApplicationCore.Extensions;
using ApplicationCore.Interfaces.Databases;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Resources;

namespace ApplicationCore.Services;

/// <summary>
/// Defines the minimum needed methods for the search services
/// </summary>
public abstract class SearchService<T> where T : class, IDatabaseSettings
{
    /// <summary>
    /// Context for connecting to the source of data
    /// </summary>
    //protected DataAccessContext<IDatabaseSettings> DataAccessContext { get; private set; }

    protected IMediathequeDbContext<T> Context { get; private set; }

    /// <summary>
    /// Transforms the POCOs into DTOs
    /// </summary>
    protected readonly IMapper _mapper;

    /// <summary>
    /// Type of search
    /// </summary>
    protected readonly string _searchType;

    /// <summary>
    /// Gives access to the texts of the app
    /// </summary>
    protected ResourceManager TextsManager { get; private set; }

    /// <summary>
    /// Constructor of the SearchService class
    /// </summary>
    /// <param name="context">Context for connecting to the source of data</param>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    /// <param name="defaultSearchType">Word used for describing the type of search if no one is found into the resources</param>
    /// <param name="resourceKey">Key used for searching the type of search into the resources</param>
    public SearchService(IMediathequeDbContext<T> context, IMapper mapper, ResourceManager textsManager, string defaultSearchType, string resourceKey)
    {
        //DataAccessContext = context;
        Context = context;
        _mapper = mapper;
        _searchType = RetrieveSearchType(defaultSearchType, resourceKey);
        TextsManager = textsManager;
    }

    /// <summary>
    /// Gets the name of the search thanks to the ResourceManager.
    /// Returns a default value if the ResourceManager doesn't give anyone
    /// </summary>
    /// <param name="defaultSearchType">The word used by default</param>
    /// <param name="resourceKey">The key used for reading the resource file</param>
    /// <returns>The word used for describing the search</returns>
    private string RetrieveSearchType(string defaultSearchType, string resourceKey)
    {
        if (TextsManager == null)
        {
            return defaultSearchType;
        }

        var name = TextsManager.GetString(resourceKey);

        return !string.IsNullOrEmpty(name) ? name : defaultSearchType;
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
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    public async Task<List<SearchResultDTO>> SearchForResults(SearchCriteriaDTO searchCriteria)
    {
        List<SearchResultDTO> searchResultsDtos = new List<SearchResultDTO>();
        List<BookResultDTO> booksList = new List<BookResultDTO>();
        List<EditionResultDTO> editionsList = new List<EditionResultDTO>();

        //if (DataAccessContext == null
        //    || DataAccessContext.WithDbContext && DataAccessContext?.DbContext?.ComplexRequests == null)
        //{
        //    throw new Exception("Le contexte d'accès aux données n'a pas été instancié");
        //}

        //var booksQuery = DataAccessContext.DbContext.ComplexRequests.GetOrderedBooksRequest(searchCriteria);
        var booksQuery = Context.ComplexRequests.GetOrderedBooksRequest(searchCriteria);

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
            //editionsList = await DataAccessContext.DbContext.ComplexRequests.GetEditionsForSeveralBooksRequest(
            editionsList = await Context.ComplexRequests.GetEditionsForSeveralBooksRequest(
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