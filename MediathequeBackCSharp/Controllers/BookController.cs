using ApplicationCore.Dtos;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MediathequeBackCSharp.Controllers
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
        /// <param name="sourceRepository">Source of data</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public BookController(IIdentifiedRepository<Book> sourceRepository, ILogger<BookController> logger, IMapper mapper) : base(sourceRepository, logger, mapper)
        {
        }
    }
}