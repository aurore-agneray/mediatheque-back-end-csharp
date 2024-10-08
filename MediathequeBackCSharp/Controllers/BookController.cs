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
    /// <remarks>
    /// Constructor of the BookController class
    /// </remarks>
    /// <param name="sourceRepository">Source of data</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="mapper">Given AutoMapper</param>
    [ApiController]
    [Route("[controller]")]
    public class BookController(IIdentifiedRepository<Book> sourceRepository, ILogger<BookController> logger, IMapper mapper) 
        : IIdentifiedController<Book, BookDTO>(sourceRepository, logger, mapper)
    {
    }
}