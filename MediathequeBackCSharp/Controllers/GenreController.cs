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
    [ApiController]
    [Route("[controller]")]
    public class GenreController : IIdentifiedController<Genre, NamedDTO>
    {
        /// <summary>
        /// Constructor of the GenreController class
        /// </summary>
        /// <param name="sourceRepository">Source of data</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public GenreController(IIdentifiedRepository<Genre> sourceRepository, ILogger<GenreController> logger, IMapper mapper) : base(sourceRepository, logger, mapper)
        {
        }
    }
}