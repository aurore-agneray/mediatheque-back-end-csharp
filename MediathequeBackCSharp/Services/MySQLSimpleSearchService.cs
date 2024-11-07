using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.MySQL.Repositories;
using MediathequeBackCSharp.Services.Abstracts;

namespace MediathequeBackCSharp.Services;

/// <summary>
/// Methods for preparing the data extracted from the MySQL database with the simple search
/// </summary>
/// <remarks>
/// Constructor of the MySQLSimpleSearchService class
/// </remarks>
/// <param name="mapper">Given AutoMapper</param>
/// <param name="textsManager">Texts manager</param>
/// <param name="repo">Repository for collecting data</param>
public class MySQLSimpleSearchService(IMapper mapper, ITextsManager textsManager, MySQLSimpleSearchRepository repo) 
    : MySQLSearchService(mapper, textsManager, repo)
{
}