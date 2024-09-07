using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Interfaces;
using AutoMapper;
using System.Resources;

namespace MediathequeBackCSharp.Services;

public abstract class BnfApiSearchService : SearchService
{
    /// <summary>
    /// Repository used for retrieving data from a given XML content
    /// </summary>
    protected readonly IXMLRepository _xmlRepository;

    /// <summary>
    /// Constructor of the BnfApiSearchService class
    /// </summary>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    /// <param name="xmlRepo">Repository for collecting data from XML content</param>
    protected BnfApiSearchService(IMapper mapper, ResourceManager textsManager, IXMLRepository xmlRepo) 
        : base(mapper, textsManager)
    {
        _xmlRepository = xmlRepo;
    }

    /// <summary>
    /// Extracts the books and their editions from the concerned XML repository
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    protected override async Task<Tuple<List<BookResultDTO>, List<EditionResultDTO>>> ExtractDataFromRepository(SearchDTO searchCriteria)
    {
        return default;
    }
}