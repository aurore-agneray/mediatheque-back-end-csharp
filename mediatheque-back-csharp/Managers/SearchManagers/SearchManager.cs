using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.DTOs.SearchDTOs.CriteriaDTOs;
using ApplicationCore.Enums;
using AutoMapper;
using mediatheque_back_csharp.Services;
using System.Resources;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the SearchController
/// </summary>
public abstract class SearchManager
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
    protected ResourceManager TextsManager { get; private set; }

    /// <summary>
    /// An object containing all available search services, injected by DI
    /// </summary>
    protected readonly AllSearchServices _services;

    /// <summary>
    /// Constructor of the SearchManager class
    /// </summary>
    /// <param name="services">An object with all available search services</param>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    public SearchManager(AllSearchServices services, IMapper mapper, ResourceManager textsManager)
    {
        _services = services;
        _mapper = mapper;
        TextsManager = textsManager;
    }

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

        if (_services is not null)
        {
            searchService = searchType switch
            {
                SearchTypeEnum.MySQLSimple => _services.MySQLSimpleSearchService,
                SearchTypeEnum.MySQLAdvanced => _services.MySQLAdvancedSearchService,
                SearchTypeEnum.BnfAPISimple => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
        }

        if (searchService is not null)
        {
            return await searchService.SearchForResults(searchCriteria);
        }
        
        return new List<SearchResultDTO>();
    }
}