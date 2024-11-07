using ApplicationCore.AbstractServices;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using AutoMapper;
using MediathequeBackCSharp.Services.Aggregates;

namespace MediathequeBackCSharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the SimpleSearchController
/// </summary>
/// <remarks>
/// Constructor of the SimpleSearchManager class
/// </remarks>
/// <param name="services">An object with all available search services</param>
/// <param name="mapper">Given AutoMapper</param>
/// <param name="textsManager">Texts manager</param>
public class SimpleSearchManager(AllSimpleSearchServices services, IMapper mapper, ITextsManager textsManager) : SearchManager<AllSimpleSearchServices>(services, mapper, textsManager)
{
    /// <summary>
    /// Gets the SIMPLE search service needed for the given type of search
    /// </summary>
    /// <param name="searchType">Type of search</param>
    /// <returns>Returns a unique SearchService object</returns>
    protected override SearchService? GetSearchService(SearchTypeEnum searchType)
    {
        if (AllSearchServices is not null)
        {
            return searchType switch
            {
                SearchTypeEnum.MySQLSimple => AllSearchServices.MySQLSimpleSearchService,
                SearchTypeEnum.BnfAPISimple => AllSearchServices.BnfApiSimpleSearchService,
                _ => throw new NotImplementedException()
            };
        }

        return default;
    }
}