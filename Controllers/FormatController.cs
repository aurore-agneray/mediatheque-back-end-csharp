using AutoMapper;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.Dtos;
using mediatheque_back_csharp.Pocos;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Format"
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class FormatController : IIdentifiedController<Format, NamedDTO>
    {
        /// <summary>
        /// Constructor of the FormatController class
        /// </summary>
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public FormatController(MediathequeDbContext context, ILogger<FormatController> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }
    }
}