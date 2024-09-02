using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Enums;

namespace ApplicationCore.Interfaces;

/// <summary>
/// A common interface for all classes of type "SearcheManager".
/// Permits to abstract the affectation of search services
/// </summary>
/// <typeparam name="T">An interface which defines objects with several services</typeparam>
public interface ISearchManager<out T> where T : class, IAllSearchServices
{
    /// <summary>
    /// Processes the search that can be of type "simple" or "advanced".
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <param name="searchType">Type of search</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    public Task<List<SearchResultDTO>> SearchForResults(SearchDTO searchCriteria, SearchTypeEnum searchType);
}