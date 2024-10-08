using ApplicationCore.AbstractServices;
using ApplicationCore.Enums;
using AutoMapper;
using MediathequeBackCSharp.Services.Aggregates;
using System.Resources;

namespace MediathequeBackCSharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the AdvancedSearchController
/// </summary>
/// <remarks>
/// Constructor of the AdvancedSearchManager class
/// </remarks>
/// <param name="services">An object with all available search services</param>
/// <param name="mapper">Given AutoMapper</param>
/// <param name="textsManager">Texts manager</param>
public class AdvancedSearchManager(AllAdvancedSearchServices services, IMapper mapper, ResourceManager textsManager) : SearchManager<AllAdvancedSearchServices>(services, mapper, textsManager)
{
    /// <summary>
    /// Gets the ADVANCED search service needed for the given type of search
    /// </summary>
    /// <param name="searchType">Type of search</param>
    /// <returns>Returns a unique SearchService object</returns>
    protected override SearchService? GetSearchService(SearchTypeEnum searchType)
    {
        if (AllSearchServices is not null)
        {
            return searchType switch
            {
                SearchTypeEnum.MySQLAdvanced => AllSearchServices.MySQLAdvancedSearchService,
                _ => throw new NotImplementedException()
            };
        }

        return default;
    }
}