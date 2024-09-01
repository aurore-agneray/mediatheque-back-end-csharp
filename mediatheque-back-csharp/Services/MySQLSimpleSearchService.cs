using ApplicationCore.Interfaces.Databases;
using AutoMapper;
using Infrastructure.MySQL.ComplexRequests;
using System.Resources;

namespace mediatheque_back_csharp.Services;

/// <summary>
/// Methods for preparing the data extracted from the MySQL database with the simple search
/// </summary>
public class MySQLSimpleSearchService : SearchService
{
    /// <summary>
    /// Constructor of the MySQLSimpleSearchService class
    /// </summary>
    /// <param name="repo">Repository for collecting data</param>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    public MySQLSimpleSearchService(MySQLSimpleSearchRepository repo, IMapper mapper, ResourceManager textsManager)
        : base((ISQLRepository<IMediathequeDbContext>)repo, mapper, textsManager)
    {
    }
}