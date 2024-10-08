using ApplicationCore.Dtos;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MediathequeBackCSharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Author"
    /// </summary>
    /// <remarks>
    /// Constructor of the AuthorController class
    /// </remarks>
    /// <param name="sourceRepository">Source of data</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="mapper">Given AutoMapper</param>
    [ApiController]
    [Route("[controller]")]
    public class AuthorController(IIdentifiedRepository<Author> sourceRepository, ILogger<AuthorController> logger, IMapper mapper) 
        : IIdentifiedController<Author, AuthorDTO>(sourceRepository, logger, mapper)
    {
    }
}