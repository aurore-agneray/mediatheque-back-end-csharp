using ApplicationCore.Dtos;
using ApplicationCore.Interfaces;
using ApplicationCore.Pocos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for the SQL table "Edition"
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class EditionController : IIdentifiedController<Edition, EditionDTO>
    {
        /// <summary>
        /// Constructor of the EditionController class
        /// </summary>
        /// <param name="sourceRepository">Source of data</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Given AutoMapper</param>
        public EditionController(IIdentifiedRepository<Edition> sourceRepository, ILogger<EditionController> logger, IMapper mapper) : base(sourceRepository, logger, mapper)
        {
        }
    }
}