using mediatheque_back_csharp.Dtos;

namespace mediatheque_back_csharp.DTOs.SearchDTOs;

/// <summary>
/// Arguments received for the search
/// </summary>
public class SearchArgsDTO
{
    /// <summary>
    /// Criterion for the simple search (Book's title, author's name, series' name or ISBN)
    /// </summary>
    public string? Criterion { get; set; }

    /// <summary>
    /// Object containing the criteria for the advanced search (title, author, genre, isbn, ...)
    /// </summary>
    public AdvancedSearchArgsDTO Criteria { get; set; }
}