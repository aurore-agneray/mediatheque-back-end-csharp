using mediatheque_back_csharp.Entities;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;

        private static readonly Author[] Authors = new[]
        {
            new Author() { Code = "ABC", FirstName = "Yo !", LastName = "POUET", Id = 1 },
            new Author() { Code = "taertgertg", FirstName = "Lola", LastName = "HEHE", Id = 2 },
            new Author() { Code = "zefzef", FirstName = "Jean", LastName = "Michel", Id = 3 },
            new Author() { Code = "rgzezer", FirstName = "Johnny", LastName = "Halliday", Id = 4 },
            new Author() { Code = "zerzerz", FirstName = "Céline", LastName = "Dion", Id = 5 },
        };

        public AuthorController(ILogger<AuthorController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAuthor")]
        public IEnumerable<Author> Get()
        {
            return Authors;
        }
    }
}