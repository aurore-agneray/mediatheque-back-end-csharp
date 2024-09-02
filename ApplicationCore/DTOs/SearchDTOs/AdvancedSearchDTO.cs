using ApplicationCore.Interfaces.DTOs;
using System.Text.Json.Serialization;

namespace ApplicationCore.DTOs.SearchDTOs;

/// <summary>
/// Arguments received for an advanced search
/// </summary>
public class AdvancedSearchDTO : SearchDTO, IAdvancedSearchDTO
{
    /// <summary>
    /// Object containing the criteria for the advanced search (title, author, genre, isbn, ...)
    /// </summary>
    [JsonPropertyName("criteria")]
    public AdvancedSearchCriteriaDTO? AdvancedCriteria { get; set; }
}