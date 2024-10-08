using ApplicationCore.Dtos;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MediathequeBackCSharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Edition"
    /// </summary>
    /// <remarks>
    /// Constructor of the EditionController class
    /// </remarks>
    /// <param name="sourceRepository">Source of data</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="mapper">Given AutoMapper</param>
    [ApiController]
    [Route("[controller]")]
    public class EditionController(IIdentifiedRepository<Edition> sourceRepository, ILogger<EditionController> logger, IMapper mapper) 
        : IIdentifiedController<Edition, EditionDTO>(sourceRepository, logger, mapper)
    {
    }
}