using mediatheque_back_csharp.Interfaces;

namespace mediatheque_back_csharp.Dtos;

/// <summary>
/// DTO for the Editions
/// </summary>
public class EditionDTO : IIdentified
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ISBN
    /// </summary>
    public string Isbn { get; set; } = null!;

    /// <summary>
    /// Ark id
    /// </summary>
    public string? ArkId { get; set; }

    /// <summary>
    /// Subtitle
    /// </summary>
    public string? Subtitle { get; set; }

    /// <summary>
    /// Publication date
    /// </summary>
    public DateTime? PublicationDate { get; set; }

    /// <summary>
    /// Publication year
    /// </summary>
    public int? PublicationYear { get; set; }

    /// <summary>
    /// Number of pages
    /// </summary>
    public int? Pages { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Volume
    /// </summary>
    public string? Volume { get; set; }

    /// <summary>
    /// Summary
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Book ID
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Format ID
    /// </summary>
    public int? FormatId { get; set; }

    /// <summary>
    /// Publisher ID
    /// </summary>
    public int PublisherId { get; set; }

    /// <summary>
    /// Series ID
    /// </summary>
    public int? SeriesId { get; set; }
}