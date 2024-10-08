using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Interfaces.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MediathequeBackCSharp.Controllers
{
    /// <summary>
    /// API requests for all entities interfacing "IIdentified"
    /// </summary>
    /// <remarks>
    /// Constructor of the IIdentifiedController class
    /// </remarks>
    /// <param name="sourceRepository">Source of data</param>
    /// <param name="logger">Given Logger</param>
    /// <param name="mapper">Transforms the entities into DTOs</param>
    [ApiController]
    public class IIdentifiedController<SourceEntity, DestDTO>(
        IIdentifiedRepository<SourceEntity> sourceRepository,
        ILogger<IIdentifiedController<SourceEntity, DestDTO>> logger,
        IMapper mapper
    ) : ControllerBase 
        where SourceEntity : class, IIdentified 
        where DestDTO : class, IIdentified
    {
        /// <summary>
        /// Source of data
        /// </summary>
        protected readonly IIdentifiedRepository<SourceEntity> _sourceRepository = sourceRepository;

        /// <summary>
        /// Logger for the IIdentifiedController
        /// </summary>
        protected readonly ILogger<IIdentifiedController<SourceEntity, DestDTO>> _logger = logger;

        /// <summary>
        /// Transforms the POCOs into DTOs
        /// </summary>
        protected readonly IMapper _mapper = mapper;

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