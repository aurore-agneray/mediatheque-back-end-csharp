using ApplicationCore.Interfaces.Databases;
using AutoMapper;
using Infrastructure.MySQL.ComplexRequests;
using MediathequeBackCSharp.Services.Abstracts;
using System.Resources;

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
public class MySQLSimpleSearchService(IMapper mapper, ResourceManager textsManager, MySQLSimpleSearchRepository repo) 
    : MySQLSearchService(mapper, textsManager, repo)
{
}