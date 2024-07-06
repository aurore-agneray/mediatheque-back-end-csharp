using mediatheque_back_csharp.Pocos;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Book"
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BookController : IIdentifiedController<Book>
    {
        /// <summary>
        /// Constructor of the BookController class
        /// </summary>
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        public BookController(MediathequeDbContext context, ILogger<BookController> logger) : base(context, logger)
        {
        }
    }
}