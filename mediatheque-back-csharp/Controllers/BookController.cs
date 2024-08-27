using ApplicationCore.Dtos;
using ApplicationCore.Pocos;
using AutoMapper;
using mediatheque_back_csharp.Database;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Book"
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BookController : IIdentifiedController<Book, BookDTO>
    {
        /// <summary>
        /// Constructor of the BookController class
        /// </summary>
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public BookController(MediathequeDbContext context, ILogger<BookController> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }
    }
}