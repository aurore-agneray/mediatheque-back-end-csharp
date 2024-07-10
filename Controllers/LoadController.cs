using AutoMapper;
using AutoMapper.QueryableExtensions;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    protected readonly MediathequeDbContext _context;

    /// <summary>
    /// Logger for the LoadController
    /// </summary>
    protected readonly ILogger<LoadController> _logger;

    /// <summary>
    /// Mapper for the LoadController
    /// </summary>
    protected readonly IMapper _mapper;

    /// <summary>
    /// Constructor of the LoadController class
    /// </summary>
    /// <param name="context">Given database context</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="mapper">Given AutoMapper</param>
    public LoadController(
        MediathequeDbContext context,
        ILogger<LoadController> logger,
        IMapper mapper
    )
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get the data used for initializing the front-end application :
    /// genres, publishers and formats lists
    /// </summary>
    /// <returns>Returns a DTO with several lists</returns>
    [HttpGet]
    public async Task<LoadDTO> Get()
    {
        var genres = await _context.Genres.OrderBy(g => g.Name)
                                          .ProjectTo<NamedDTO>(_mapper.ConfigurationProvider)
                                          .ToListAsync();

        var publishers = await _context.Publishers.OrderBy(p => p.PublishingHouse)
                                                  .ProjectTo<NamedDTO>(_mapper.ConfigurationProvider)
                                                  .ToListAsync();

        var formats = await _context.Formats.OrderBy(f => f.Name)
                                            .ProjectTo<NamedDTO>(_mapper.ConfigurationProvider)
                                            .ToListAsync();

        return new LoadDTO
        {
            Genres = genres,
            Publishers = publishers,
            Formats = formats
        };
    }
}