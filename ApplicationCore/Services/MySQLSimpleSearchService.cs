using ApplicationCore.AbstractClasses;
using ApplicationCore.DatabasesSettings;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Texts;
using AutoMapper;
using System.Resources;

namespace ApplicationCore.Services;

/// <summary>
/// Methods for preparing the data sent by the SimpleSearchController
/// </summary>
public class MySQLSimpleSearchService : SearchService<MySQLDatabaseSettings>
{
    /// <summary>
    /// Constructor of the MySQLSimpleSearchService class
    /// </summary>
    /// <param name="context">Context for connecting to the source of data</param>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    public MySQLSimpleSearchService(IMediathequeDbContext<MySQLDatabaseSettings> context, IMapper mapper, ResourceManager textsManager)
        : base(context, mapper, textsManager, Constants.SIMPLE_SEARCH_TYPE, TextsKeys.SIMPLE_SEARCH_TYPE)
    {
    }
}