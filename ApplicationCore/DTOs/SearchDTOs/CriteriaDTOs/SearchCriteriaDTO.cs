using System.Text.Json.Serialization;

namespace ApplicationCore.DTOs.SearchDTOs.CriteriaDTOs;

/// <summary>
/// Arguments received for the search
/// </summary>
public class SearchCriteriaDTO
{
    /// <summary>
    /// Criterion for the simple search (Book's title, author's name, series' name or ISBN)
    /// </summary>
    [JsonPropertyName("criterion")]
    public string? SimpleCriterion { get; set; }

    /// <summary>
    /// Object containing the criteria for the advanced search (title, author, genre, isbn, ...)
    /// </summary>
    [JsonPropertyName("criteria")]
    public AdvancedSearchCriteriaDTO? AdvancedCriteria { get; set; }
}