using mediatheque_back_csharp.Pocos;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Author"
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        /// <summary>
        /// Context for connecting to the database
        /// </summary>
        private readonly MediathequeDbContext _context;

        /// <summary>
        /// Logger for the AuthorController
        /// </summary>
        private readonly ILogger<AuthorController> _logger;

        /// <summary>
        /// Raw list of authors for testing
        /// </summary>
        private static readonly Author[] Authors =
        {
            new() { Code = "ABC", FirstName = "Yo !", LastName = "POUET", Id = 1 },
            new() { Code = "taertgertg", FirstName = "Lola", LastName = "HEHE", Id = 2 },
            new() { Code = "zefzef", FirstName = "Jean", LastName = "Michel", Id = 3 },
            new() { Code = "rgzezer", FirstName = "Johnny", LastName = "Halliday", Id = 4 },
            new() { Code = "zerzerz", FirstName = "Céline", LastName = "Dion", Id = 5 },
        };

        /// <summary>
        /// Constructor of the AuthorController class
        /// </summary>
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        public AuthorController(MediathequeDbContext context, ILogger<AuthorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all CRUD request for the Authors
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAuthors")]
        public IEnumerable<Author> Get()
        {
            return this._context.Authors;
        }
    }
}