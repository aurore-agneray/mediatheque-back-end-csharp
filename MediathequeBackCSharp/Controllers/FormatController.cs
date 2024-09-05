using ApplicationCore.Dtos;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MediathequeBackCSharp.Controllers
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
        /// <param name="sourceRepository">Source of data</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public FormatController(IIdentifiedRepository<Format> sourceRepository, ILogger<FormatController> logger, IMapper mapper) : base(sourceRepository, logger, mapper)
        {
        }
    }
}