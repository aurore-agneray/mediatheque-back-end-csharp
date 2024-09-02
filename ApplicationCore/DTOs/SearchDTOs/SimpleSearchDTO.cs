using ApplicationCore.Interfaces;
using System.Text.Json.Serialization;

namespace ApplicationCore.DTOs.SearchDTOs;

/// <summary>
/// Arguments received for a simple search
/// </summary>
public class SimpleSearchDTO : SearchDTO, ISimpleSearchDTO
{
    /// <summary>
    /// Criterion for the simple search (Book's title, author's name, series' name or ISBN)
    /// </summary>
    [JsonPropertyName("criterion")]
    public string? SimpleCriterion { get; set; }
}