using mediatheque_back_csharp.Pocos;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Publisher"
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PublisherController : IIdentifiedController<Publisher>
    {
        /// <summary>
        /// Constructor of the PublisherController class
        /// </summary>
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        public PublisherController(MediathequeDbContext context, ILogger<PublisherController> logger) : base(context, logger)
        {
        }
    }
}