using ApplicationCore.Interfaces;
using System.Text.Json.Serialization;

namespace ApplicationCore.DTOs.SearchDTOs;

/// <summary>
/// General DTO transmitted by all types of search
/// </summary>
public class SearchDTO : ISearchDTO
{
    /// <summary>
    /// Indicates if the app need to connect to the BnF API or not
    /// </summary>
    [JsonPropertyName("apiBnf")]
    public bool UseBnfApi { get; set; }

    /// <summary>
    /// Number of notices returned by the BnF API
    /// </summary>
    [JsonPropertyName("apiBnfNoticesQty")]
    public int BnfNoticesQuantity { get; set; }
}