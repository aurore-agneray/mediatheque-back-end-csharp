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
        /// Logger for the AuthorController
        /// </summary>
        private readonly ILogger<AuthorController> _logger;

        /// <summary>
        /// Raw list of authors for testing
        /// </summary>
        private static readonly Author[] Authors = new[]
        {
            new Author() { Code = "ABC", FirstName = "Yo !", LastName = "POUET", Id = 1 },
            new Author() { Code = "taertgertg", FirstName = "Lola", LastName = "HEHE", Id = 2 },
            new Author() { Code = "zefzef", FirstName = "Jean", LastName = "Michel", Id = 3 },
            new Author() { Code = "rgzezer", FirstName = "Johnny", LastName = "Halliday", Id = 4 },
            new Author() { Code = "zerzerz", FirstName = "Céline", LastName = "Dion", Id = 5 },
        };

        /// <summary>
        /// Constructor of the AuthorController class
        /// </summary>
        /// <param name="logger">Given Logger</param>
        public AuthorController(ILogger<AuthorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get all CRUD request for the Authors
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAuthors")]
        public IEnumerable<Author> Get()
        {
            return Authors;
        }
    }
}