using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Extensions;
using AutoMapper;
using System.Resources;

namespace MediathequeBackCSharp.Services;

/// <summary>
/// Defines the minimum needed methods for the search services
/// </summary>
public abstract class SearchService
{
    /// <summary>
    /// Transforms the POCOs into DTOs
    /// </summary>
    protected readonly IMapper _mapper;

    /// <summary>
    /// Gives access to the texts of the app
    /// </summary>
    protected ResourceManager TextsManager { get; private set; }

    /// <summary>
    /// Constructor of the SearchService class
    /// </summary>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    public SearchService(IMapper mapper, ResourceManager textsManager)
    {
        _mapper = mapper;
        TextsManager = textsManager;
    }

    /// <summary>
    /// Extracts the books and their editions from the concerned repository
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    protected abstract Task<Tuple<List<BookResultDTO>, List<EditionResultDTO>>> ExtractDataFromRepository(SearchDTO searchCriteria);

    /// <summary>
    /// Gets the name of the search thanks to the ResourceManager.
    /// Returns a default value if the ResourceManager doesn't give anyone
    /// </summary>
    /// <param name="defaultSearchType">The word used by default</param>
    /// <param name="resourceKey">The key used for reading the resource file</param>
    /// <returns>The word used for describing the search</returns>
    private string RetrieveSearchType(string defaultSearchType, string resourceKey)
    {
        if (TextsManager == null)
        {
            return defaultSearchType;
        }

        var name = TextsManager.GetString(resourceKey);

        return !string.IsNullOrEmpty(name) ? name : defaultSearchType;
    }

    /// <summary>
    /// Order the given list by volumes' alphanumerical ascending order
    /// </summary>
    /// <param name="editions">List of EditionResultDTO objects</param>
    /// <returns>Ordered list of EditionResultDTO objects</returns>
    protected List<EditionResultDTO> OrderEditionsByVolume(IEnumerable<EditionResultDTO> editions)
    {
        return editions.OrderBy(item => item?.Volume != null ? item.Volume.ExtractPrefix() : string.Empty)
                       .ThenBy(item => item?.Volume != null ? item.Volume.ExtractNumber() : 0)
                       .ToList();
    }

    /// <summary>
    /// Groups the editions of the given list into a dictionary 
    /// where the keys are the series' names
    /// </summary>
    /// <param name="editions">List of EditionResultDTOs objects</param>
    /// <returns>Returns a dictionary where the keys are the series' names
    /// and the elements are some lists containing editions</returns>
    protected Dictionary<string, List<EditionResultDTO>> GroupEditionsBySeriesName(IEnumerable<EditionResultDTO> editions)
    {
        if (editions == null || editions.Count() == 0)
        {
            return new Dictionary<string, List<EditionResultDTO>>();
        }

        return editions.GroupBy(ed =>
        {
            if (ed?.Series?.SeriesName != null)
            {
                return ed.Series.SeriesName;
            }

            return "0";

        }).ToDictionary(
            group => group.Key,
            group => group.ToList()
        );
    }

    /// <summary>
    /// Processes the search that can be of type "simple" or "advanced".
    /// The process depends on the source of data
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    public async Task<List<SearchResultDTO>> SearchForResults(SearchDTO searchCriteria)
    {
        List<SearchResultDTO> searchResultsDtos = new List<SearchResultDTO>();
        List<BookResultDTO> booksList = new List<BookResultDTO>();
        List<EditionResultDTO> editionsList = new List<EditionResultDTO>();

        (booksList, editionsList) = await this.ExtractDataFromRepository(searchCriteria);

        // Constructs the final DTOs
        if (editionsList != null && editionsList.Any())
        {
            searchResultsDtos = booksList.Select(dto =>
                new SearchResultDTO(dto)
            ).ToList();

            searchResultsDtos.ForEach(rDto => {
                rDto.Editions = GroupEditionsBySeriesName(
                    OrderEditionsByVolume(
                        editionsList.Where(ed => ed.BookId == rDto.BookId)
                    )
                );
            });
        }

        return searchResultsDtos;
    }
}