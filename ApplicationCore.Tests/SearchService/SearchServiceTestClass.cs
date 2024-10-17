using ApplicationCore.DTOs.SearchDTOs;
using AutoMapper;
using System.Resources;

namespace ApplicationCore.Tests.SearchService;

internal class SearchServiceTestClass : ApplicationCore.AbstractServices.SearchService
{
    public SearchServiceTestClass(IMapper mapper, ResourceManager textsManager) : base(mapper, textsManager)
    {
    }

    protected override Task<Tuple<List<BookResultDTO>, List<EditionResultDTO>>> ExtractDataFromRepository(SearchDTO searchCriteria)
    {
        throw new NotImplementedException();
    }
}