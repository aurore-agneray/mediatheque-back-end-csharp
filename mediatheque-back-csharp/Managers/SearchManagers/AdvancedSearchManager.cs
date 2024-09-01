using AutoMapper;
using mediatheque_back_csharp.Services;
using System.Resources;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the AdvancedSearchController
/// </summary>
public class AdvancedSearchManager : SearchManager
{
    /// <summary>
    /// Constructor of the AdvancedSearchManager class
    /// </summary>
    /// <param name="services">An object with all available search services</param>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    public AdvancedSearchManager(AllSearchServices services, IMapper mapper, ResourceManager textsManager) 
        : base(services, mapper, textsManager)
    {
    }
}