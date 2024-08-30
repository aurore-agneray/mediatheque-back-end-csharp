using ApplicationCore.Dtos;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Publisher"
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PublisherController : IIdentifiedController<Publisher, PublisherDTO>
    {
        /// <summary>
        /// Constructor of the PublisherController class
        /// </summary>
        /// <param name="sourceRepository">Source of data</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public PublisherController(IIdentifiedRepository<Publisher> sourceRepository, ILogger<PublisherController> logger, IMapper mapper) : base(sourceRepository, logger, mapper)
        {
        }
    }
}