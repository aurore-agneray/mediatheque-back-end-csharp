using ApplicationCore.Enums;
using AutoMapper;
using mediatheque_back_csharp.Services;
using System.Resources;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the SimpleSearchController
/// </summary>
public class SimpleSearchManager : SearchManager<AllSimpleSearchServices>
{
    /// <summary>
    /// Constructor of the SimpleSearchManager class
    /// </summary>
    /// <param name="services">An object with all available search services</param>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    public SimpleSearchManager(AllSimpleSearchServices services, IMapper mapper, ResourceManager textsManager)
        : base(services, mapper, textsManager)
    {
    }

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
                _ => throw new NotImplementedException()
            };
        }

        return default;
    }
}