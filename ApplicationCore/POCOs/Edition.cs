using ApplicationCore.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Pocos;

/// <summary>
/// POCO for the Editions
/// </summary>
[Table("Edition")]
public class Edition : IIdentified
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    [Column("ID")]
    public int Id { get; set; }

    /// <summary>
    /// ISBN
    /// </summary>
    [Column("ISBN")]
    public string Isbn { get; set; } = null!;

    /// <summary>
    /// Ark id
    /// </summary>
    [Column("ark_id")]
    public string? ArkId { get; set; }

    /// <summary>
    /// Subtitle
    /// </summary>
    [Column("subtitle")]
    public string? Subtitle { get; set; }

    /// <summary>
    /// Publication date
    /// </summary>
    [Column("publication_date")]
    public DateTime? PublicationDate { get; set; }

    /// <summary>
    /// Publication year
    /// </summary>
    [Column("publication_year")]
    public int? PublicationYear { get; set; }

    /// <summary>
    /// Number of pages
    /// </summary>
    [Column("pages")]
    public int? Pages { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    [Column("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Volume
    /// </summary>
    [Column("volume")]
    public string? Volume { get; set; }

    /// <summary>
    /// Summary
    /// </summary>
    [Column("summary")]
    public string? Summary { get; set; }

    /// <summary>
    /// Book ID
    /// </summary>
    [Column("book_ID")]
    public int BookId { get; set; }

    /// <summary>
    /// Format ID
    /// </summary>
    [Column("format_ID")]
    public int? FormatId { get; set; }

    /// <summary>
    /// Publisher ID
    /// </summary>
    [Column("pub_ID")]
    public int PublisherId { get; set; }

    /// <summary>
    /// Series ID
    /// </summary>
    [Column("series_ID")]
    public int? SeriesId { get; set; }

    /// <summary>
    /// Book
    /// </summary>
    public virtual Book Book { get; set; } = null!;

    /// <summary>
    /// Format
    /// </summary>
    public virtual Format? Format { get; set; }

    /// <summary>
    /// Publisher
    /// </summary>
    public virtual Publisher Publisher { get; set; } = null!;

    /// <summary>
    /// Series
    /// </summary>
    public virtual Series? Series { get; set; }
}