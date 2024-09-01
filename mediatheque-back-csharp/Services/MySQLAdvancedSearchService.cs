using ApplicationCore.Interfaces.Databases;
using AutoMapper;
using Infrastructure.MySQL.ComplexRequests;
using System.Resources;

namespace mediatheque_back_csharp.Services;

/// <summary>
/// Methods for preparing the data extracted from the MySQL database with the advanced search
/// </summary>
public class MySQLAdvancedSearchService : SearchService
{
    /// <summary>
    /// Constructor of the MySQLSimpleSearchService class
    /// </summary>
    /// <param name="repo">Repository for collecting data</param>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    public MySQLAdvancedSearchService(MySQLAdvancedSearchRepository repo, IMapper mapper, ResourceManager textsManager)
        : base((ISQLRepository<IMediathequeDbContextFields>)repo, mapper, textsManager)
    {
    }
}