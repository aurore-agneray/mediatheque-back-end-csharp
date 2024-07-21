using mediatheque_back_csharp.DTOs.SearchDTOs;

namespace mediatheque_back_csharp.Interfaces;

/// <summary>
/// Represents services used for retrieving data
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Indicates if it is an advanced or a simple search
    /// </summary>
    bool IsAdvanced { get; set; }

    /// <summary>
    /// Retrieves and returns the results of the wanted search
    /// </summary>
    /// <param name="criterion">
    /// The type of that property depends on the type of
    /// search. For the BnF search it will be a string.
    /// For the MySQL database it will be an array to send to the ORM.
    /// </param>
    /// <param name="noticesNb">Max number of notices returned by the BnF</param>
    /// <returns>A list of SearchResultDTOs</returns>
    Task<IEnumerable<SearchResultDTO>> GetResults(object criterion, int noticesNb);
}