using AutoMapper;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.DTOs.SearchDTOs;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the AdvancedSearchController
/// </summary>
public class AdvancedSearchManager : SearchManager
{
    /// <summary>
    /// Constructor of the AdvancedSearchManager class
    /// </summary>
    /// <param name="context">HTTP Context</param>
    /// <param name="mapper">Given AutoMapper</param>
    public AdvancedSearchManager(MediathequeDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    /// <summary>
    /// Processes the advanced search
    /// </summary>
    /// <param name="criteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    public override async Task<List<SearchResultDTO>> SearchForResults(SearchArgsDTO criteria)
    {
        return await Task.Run(async() => {
            var test = criteria?.Title;
            return new List<SearchResultDTO>() {
                new SearchResultDTO(new BookResultDTO() {
                    Title = test
                }) {
                    BookId = 1
                }
            };
        });
    }
}