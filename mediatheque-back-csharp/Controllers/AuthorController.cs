using ApplicationCore.Dtos;
using ApplicationCore.Pocos;
using AutoMapper;
using Infrastructure.MySQL;
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
        /// <param name="context">Given database context</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public AuthorController(MySQLDbContext context, ILogger<AuthorController> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }
    }
}