using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Databases;
using AutoMapper;
using Infrastructure.MySQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mediatheque_back_csharp.Controllers
{
    /// <summary>
    /// API requests for all entities interfacing "IIdentified"
    /// </summary>
    [ApiController]
    public class IIdentifiedController<SourceEntity, DestDTO> : ControllerBase 
        where SourceEntity : class, IIdentified 
        where DestDTO : class, IIdentified
    {
        /// <summary>
        /// Source of data
        /// </summary>
        protected readonly IIdentifiedRepository<SourceEntity> _sourceRepository;

        /// <summary>
        /// Logger for the IIdentifiedController
        /// </summary>
        protected readonly ILogger<IIdentifiedController<SourceEntity, DestDTO>> _logger;

        /// <summary>
        /// Transforms the POCOs into DTOs
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Constructor of the IIdentifiedController class
        /// </summary>
        /// <param name="sourceRepository">Source of data</param>
        /// <param name="logger">Given Logger</param>
        /// <param name="mapper">Transforms the entities into DTOs</param>
        public IIdentifiedController(
            IIdentifiedRepository<SourceEntity> sourceRepository,
            ILogger<IIdentifiedController<SourceEntity, DestDTO>> logger,
            IMapper mapper)
        {
            _sourceRepository = sourceRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all CRUD request for the entities of IIdentified type.
        /// </summary>
        /// <returns>List of some IIdentified objects of the database</returns>
        [HttpGet]
        public async Task<IEnumerable<DestDTO>> GetAll()
        {
            var pocosList = await _sourceRepository.GetAll();
            return _mapper.Map<IEnumerable<SourceEntity>, List<DestDTO>>(pocosList);
        }
    }
}