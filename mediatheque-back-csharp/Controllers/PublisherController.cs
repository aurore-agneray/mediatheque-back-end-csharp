using ApplicationCore.Dtos;
using ApplicationCore.Pocos;
using AutoMapper;
using Infrastructure.MySQL;
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
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public PublisherController(MySQLDbContext context, ILogger<PublisherController> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }
    }
}