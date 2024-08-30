using ApplicationCore.Dtos;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Author"
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : IIdentifiedController<Author, AuthorDTO>
    {
        /// <summary>
        /// Constructor of the AuthorController class
        /// </summary>
        /// <param name="sourceRepository">Source of data</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public AuthorController(IIdentifiedRepository<Author> sourceRepository, ILogger<AuthorController> logger, IMapper mapper) : base(sourceRepository, logger, mapper)
        {
        }
    }
}