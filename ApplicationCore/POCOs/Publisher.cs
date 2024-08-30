using ApplicationCore.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Pocos;

/// <summary>
/// POCO for the Publishers
/// </summary>
[Table("Publisher")]
public class Publisher : IIdentified
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    [Column("ID")]
    public int Id { get; set; }

    /// <summary>
    /// Custom code
    /// </summary>
    [Column("pub_code")]
    public string Code { get; set; } = null!;

    /// <summary>
    /// Publishing house name
    /// </summary>
    [Column("publishing_house")]
    public string? PublishingHouse { get; set; }

    /// <summary>
    /// Country name
    /// </summary>
    [Column("country")]
    public string? Country { get; set; }

    /// <summary>
    /// Year of establishment
    /// </summary>
    [Column("year_established")]
    public int? YearEstablished { get; set; }

    /// <summary>
    /// List of editions
    /// </summary>
    public virtual ICollection<Edition> Editions { get; set; } = new List<Edition>();
}