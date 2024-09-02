using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.DTOs.SearchDTOs.CriteriaDTOs;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using AutoMapper;
using mediatheque_back_csharp.Services;
using System.Resources;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the SearchController
/// </summary>
/// <typeparam name="T">Defines the type of the available services for the concerned manager</typeparam>
public abstract class SearchManager<T> : ISearchManager<T> where T : class, IAllSearchServices
{
    /// <summary>
    /// Transforms the POCOs into DTOs
    /// </summary>
    protected readonly IMapper _mapper;

    /// <summary>
    /// Type of search
    /// </summary>
    protected readonly SearchTypeEnum _searchType;

    /// <summary>
    /// Gives access to the texts of the app
    /// </summary>
    protected T AllSearchServices { get; set; }

    /// <summary>
    /// Gives access to the texts of the app
    /// </summary>
    protected ResourceManager TextsManager { get; private set; }

    /// <summary>
    /// Constructor of the SearchManager class
    /// </summary>
    /// <param name="services">An object with all available search services</param>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    public SearchManager(T services, IMapper mapper, ResourceManager textsManager)
    {
        AllSearchServices = services;
        _mapper = mapper;
        TextsManager = textsManager;
    }

    /// <summary>
    /// Gets the search service needed for the given type of search
    /// </summary>
    /// <param name="searchType">Type of search</param>
    /// <returns>Returns a unique SearchService object</returns>
    protected abstract SearchService? GetSearchService(SearchTypeEnum searchType);

    /// <summary>
    /// Processes the search that can be of type "simple" or "advanced".
    /// The difference is defined by the "GetOrderedBooksRequest" call.
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <param name="searchType">Type of search</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    public async Task<List<SearchResultDTO>> SearchForResults(SearchCriteriaDTO searchCriteria, SearchTypeEnum searchType)
    {
        SearchService? searchService = null;

        if (this.TextsManager == null)
        {
            throw new ArgumentNullException(nameof(this.TextsManager));
        }

        searchService = GetSearchService(searchType);

        if (searchService is not null)
        {
            return await searchService.SearchForResults(searchCriteria);
        }
        
        return new List<SearchResultDTO>();
    }
}