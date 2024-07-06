using mediatheque_back_csharp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for all entities interfacing "IIdentified"
    /// </summary>
    [ApiController]
    [Route("entity")]
    public class IIdentifiedController : ControllerBase
    {
        /// <summary>
        /// Context for connecting to the database
        /// </summary>
        private readonly MediathequeDbContext _context;

        /// <summary>
        /// Logger for the IIdentifiedController
        /// </summary>
        private readonly ILogger<IIdentifiedController> _logger;

        /// <summary>
        /// Constructor of the IIdentifiedController class
        /// </summary>
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        public IIdentifiedController(MediathequeDbContext context, ILogger<IIdentifiedController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all CRUD request for the IIdentified
        /// </summary>
        /// <returns>List of some IIdentified objects of the database (here the authors, the series and the formats)</returns>
        [HttpGet(Name = "GetEntities")]
        public IEnumerable<IIdentified> Get()
        {
            List<IIdentified> output = new List<IIdentified>(this._context.Authors);
            output.AddRange(this._context.Books);
            output.AddRange(this._context.Editions);
            output.AddRange(this._context.Formats);
            output.AddRange(this._context.Genres);
            output.AddRange(this._context.Publishers);
            output.AddRange(this._context.Series);

            return output;
        }
    }
}