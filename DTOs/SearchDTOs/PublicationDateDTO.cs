namespace mediatheque_back_csharp.DTOs.SearchDTOs;

/// <summary>
/// Represents the criterion given into the front-end
/// for comparing the publication dates
/// </summary>
public class PublicationDateDTO {

    /// <summary>
    /// "=", "<" or ">"
    /// </summary>
    public string? Operator { get; set; }
    
    /// <summary>
    /// A date into this format : "2024-07-17T16:24:00.000Z"
    /// </summary>
    public string? Criterion { get; set; }
}