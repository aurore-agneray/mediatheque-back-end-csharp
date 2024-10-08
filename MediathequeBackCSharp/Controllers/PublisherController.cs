using ApplicationCore.Dtos;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MediathequeBackCSharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Publisher"
    /// </summary>
    /// <remarks>
    /// Constructor of the PublisherController class
    /// </remarks>
    /// <param name="sourceRepository">Source of data</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="mapper">Given AutoMapper</param>
    [ApiController]
    [Route("[controller]")]
    public class PublisherController(IIdentifiedRepository<Publisher> sourceRepository, ILogger<PublisherController> logger, IMapper mapper) : IIdentifiedController<Publisher, PublisherDTO>(sourceRepository, logger, mapper)
    {
    }
}