using ApplicationCore.Dtos;
using AutoMapper;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.Pocos;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
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
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public GenreController(MediathequeDbContext context, ILogger<GenreController> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }
    }
}