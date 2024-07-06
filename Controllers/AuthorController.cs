using mediatheque_back_csharp.Interfaces;
using mediatheque_back_csharp.Pocos;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Author"
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : IIdentifiedController<Author>
    {
        /// <summary>
        /// Constructor of the AuthorController class
        /// </summary>
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        public AuthorController(MediathequeDbContext context, ILogger<AuthorController> logger) : base(context, logger)
        {
        }
    }
}