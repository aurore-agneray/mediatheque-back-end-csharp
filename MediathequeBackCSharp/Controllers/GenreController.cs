using ApplicationCore.Dtos;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MediathequeBackCSharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Genre"
    /// </summary>
    /// <remarks>
    /// Constructor of the GenreController class
    /// </remarks>
    /// <param name="sourceRepository">Source of data</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="mapper">Given AutoMapper</param>
    [ApiController]
    [Route("[controller]")]
    public class GenreController(IIdentifiedRepository<Genre> sourceRepository, ILogger<GenreController> logger, IMapper mapper) 
        : IIdentifiedController<Genre, NamedDTO>(sourceRepository, logger, mapper)
    {
    }
}