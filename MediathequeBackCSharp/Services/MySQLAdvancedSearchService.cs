using ApplicationCore.Enums;
using ApplicationCore.Interfaces.Databases;
using AutoMapper;
using Infrastructure.MySQL.ComplexRequests;
using System.Resources;

namespace MediathequeBackCSharp.Services;

/// <summary>
/// Methods for preparing the data extracted from the MySQL database with the advanced search
/// </summary>
public class MySQLAdvancedSearchService : SearchService
{
    /// <summary>
    /// Constructor of the MySQLSimpleSearchService class
    /// </summary>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    /// <param name="repo">Repository for collecting data</param>
    public MySQLAdvancedSearchService(IMapper mapper, ResourceManager textsManager, MySQLAdvancedSearchRepository repo)
        : base(mapper, textsManager, SourceTypeEnum.SQL, (ISQLRepository<IMediathequeDbContextFields>)repo)
    {
    }
}