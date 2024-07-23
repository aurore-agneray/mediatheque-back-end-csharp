using mediatheque_back_csharp.Dtos;

namespace mediatheque_back_csharp.DTOs.SearchDTOs;

/// <summary>
/// Arguments received for the advanced search
/// </summary>
public class AdvancedSearchArgsDTO
{
    /// <summary>
    /// Book's title
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Edition's ISBN
    /// </summary>
    public string? Isbn { get; set; }

    /// <summary>
    /// Author's name
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// Series' name
    /// </summary>
    public string? Series { get; set; }
    
    /// <summary>
    /// Book's genre (ID + name)
    /// </summary>
    public NamedDTO? Genre { get; set; }
    
    /// <summary>
    /// Publisher (ID + name)
    /// </summary>
    public NamedDTO? Publisher { get; set; }
    
    /// <summary>
    /// Format (ID + name)
    /// </summary>
    public NamedDTO? Format { get; set; }
    
    /// <summary>
    /// An object with an operator used for comparing dates
    /// and a date into this format : "2024-07-17T16:24:00.000Z"
    /// </summary>
    public PublicationDateDTO? PubDate { get; set; }
}