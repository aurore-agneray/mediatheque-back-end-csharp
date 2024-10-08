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
    /// <remarks>
    /// Constructor of the FormatController class
    /// </remarks>
    /// <param name="sourceRepository">Source of data</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="mapper">Given AutoMapper</param>
    [ApiController]
    [Route("[controller]")]
    public class FormatController(IIdentifiedRepository<Format> sourceRepository, ILogger<FormatController> logger, IMapper mapper) 
        : IIdentifiedController<Format, NamedDTO>(sourceRepository, logger, mapper)
    {
    }
}