using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.MySQL;
using mediatheque_back_csharp.Classes;
using mediatheque_back_csharp.Texts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Resources;

namespace mediatheque_back_csharp.Controllers;

/// <summary>
/// API requests for the loading of the search front-end application
/// </summary>
[ApiController]
[Route("/load")]
public class LoadController : ControllerBase
{
    /// <summary>
    /// Context for connecting to the database
    /// </summary>
    protected readonly MySQLDbContext _context;

    /// <summary>
    /// Logger for the LoadController
    /// </summary>
    protected readonly ILogger<LoadController> _logger;

    /// <summary>
    /// Mapper for the LoadController
    /// </summary>
    protected readonly IMapper _mapper;

    /// <summary>
    /// Gives access to the texts of the app
    /// </summary>
    protected readonly ResourceManager _textsManager;

    /// <summary>
    /// Constructor of the LoadController class
    /// </summary>
    /// <param name="context">Given database context</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    public LoadController(
        MySQLDbContext context,
        ILogger<LoadController> logger,
        IMapper mapper, 
        ResourceManager textsManager
    )
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _textsManager = textsManager;
    }

    /// <summary>
    /// Get the data used for initializing the front-end application :
    /// genres, publishers and formats lists
    /// </summary>
    /// <returns>Returns a DTO with several lists</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Task<List<NamedDTO>> ProjectDbINamedList(IOrderedQueryable<INamed> list)
        {
            return list.ProjectTo<NamedDTO>(_mapper.ConfigurationProvider)
                       .ToListAsync();
        }

        if (_textsManager == null)
        {
            return new ErrorObject(
                HttpStatusCode.InternalServerError,
                Constants.ERROR_TEXTS_RESOURCES_READING
            );
        }

        if (!_context.IsDatabaseAvailable())
        {
            var errorMessage = _textsManager.GetString(TextsKeys.ERROR_DATABASE_CONNECTION) ?? string.Empty;

            return new ErrorObject(
                HttpStatusCode.InternalServerError,
                errorMessage
            );
        }

        try
        {
            var results = new LoadDTO
            {
                Genres = await ProjectDbINamedList(_context.Genres.OrderBy(g => g.Name)),
                Publishers = await _context.Publishers.OrderBy(p => p.PublishingHouse)
                                                      .ProjectTo<NamedDTO>(_mapper.ConfigurationProvider)
                                                      .ToListAsync(),
                Formats = await ProjectDbINamedList(_context.Formats.OrderBy(f => f.Name))
            };

            return Ok(results);
        }
        catch (Exception ex)
        {
            return new ErrorObject(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}