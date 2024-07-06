using mediatheque_back_csharp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for all entities interfacing "IIdentified"
    /// </summary>
    [ApiController]
    public class IIdentifiedController<TEntity> : ControllerBase where TEntity : class, IIdentified
    {
        /// <summary>
        /// Context for connecting to the database
        /// </summary>
        protected readonly MediathequeDbContext _context;

        /// <summary>
        /// Logger for the IIdentifiedController
        /// </summary>
        protected readonly ILogger<IIdentifiedController<TEntity>> _logger;

        /// <summary>
        /// Constructor of the IIdentifiedController class
        /// </summary>
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="isAParentController">Indicates if we want to instanciate a IIdentifiedController or a child</param>
        public IIdentifiedController(MediathequeDbContext context, ILogger<IIdentifiedController<TEntity>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get CRUD request for the TEntity.
        /// </summary>
        /// <returns>List of some IIdentified objects of the database</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            //List<IIdentified> output = new List<IIdentified>(this._context.Authors);
            //output.AddRange(this._context.Books);
            //output.AddRange(this._context.Editions);
            //output.AddRange(this._context.Formats);
            //output.AddRange(this._context.Genres);
            //output.AddRange(this._context.Publishers);
            //output.AddRange(this._context.Series);

            //return output;
            return await this._context.Set<TEntity>().ToListAsync();
        }
    }
}