using ApplicationCore;
using ApplicationCore.Dtos;
using ApplicationCore.Interfaces.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.MySQL.Repositories;
using MediathequeBackCSharp.Classes;
using MediathequeBackCSharp.Controllers;
using MediathequeBackCSharp.Texts;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Resources;

namespace MediathequeBackCSharp.Managers;

/// <summary>
/// Methods for preparing the data sent by the LoadController
/// </summary>
/// <param name="repo">Data source</param>
/// <param name="logger">Given Logger</param>
/// <param name="mapper">AutoMapper</param>
/// <param name="textsManager">Texts manager</param>
public class LoadManager(MySQLLoadRepository repo, ILogger<LoadController> logger, IMapper mapper, ResourceManager textsManager)
{
    /// <summary>
    /// Repository for querying the database
    /// </summary>
    public readonly MySQLLoadRepository _repository = repo;

    /// <summary>
    /// Logger
    /// </summary>
    public readonly ILogger<LoadController> _logger = logger;

    /// <summary>
    /// Mapper
    /// </summary>
    public readonly IMapper _mapper = mapper;

    /// <summary>
    /// Gives access to the texts of the app
    /// </summary>
    public readonly ResourceManager _textsManager = textsManager;

    private Task<List<NamedDTO>> ProjectDbINamedList(IOrderedQueryable<INamed> list)
    {
        return list.ProjectTo<NamedDTO>(_mapper.ConfigurationProvider)
                   .ToListAsync();
    }

    /// <summary>
    /// Checks the values of the properties set into the manager
    /// </summary>
    /// <returns>Returns an ErrorObjedct if an issue is detected. Or null</returns>
    public ErrorObject? CheckProperties()
    {
        if (_textsManager == null)
        {
            return new ErrorObject(
                HttpStatusCode.InternalServerError,
                Constants.ERROR_TEXTS_RESOURCES_READING
            );
        }

        if (!_repository.IsDatabaseAvailable())
        {
            var errorMessage = _textsManager.GetString(TextsKeys.ERROR_DATABASE_CONNECTION) ?? string.Empty;

            return new ErrorObject(
                HttpStatusCode.InternalServerError,
                errorMessage
            );
        }

        return null;
    }

    /// <summary>
    /// Returns the results for the "Get" method
    /// </summary>
    public async Task<LoadDTO> Get()
    {
        return new LoadDTO
        {
            Genres = await ProjectDbINamedList(_repository.GetGenres().OrderBy(g => g.Name)),
            Publishers = await _repository.GetPublishers().OrderBy(p => p.PublishingHouse)
                                                      .ProjectTo<NamedDTO>(_mapper.ConfigurationProvider)
                                                      .ToListAsync(),
            Formats = await ProjectDbINamedList(_repository.GetFormats().OrderBy(f => f.Name))
        };
    }
}