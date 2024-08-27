using ApplicationCore.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Pocos;

/// <summary>
/// POCO for the Series
/// </summary>
[Table("Series")]
public class Series : INamed
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    [Column("ID")]
    public int Id { get; set; }

    /// <summary>
    /// Custom code
    /// </summary>
    [Column("series_code")]
    public string Code { get; set; } = null!;

    /// <summary>
    /// Name
    /// </summary>
    [Column("series_name")]
    public string? Name { get; set; }

    /// <summary>
    /// Number of planned volumes
    /// </summary>
    [Column("planned_volumes")]
    public int? PlannedVolumes { get; set; }

    /// <summary>
    /// Publisher ID
    /// </summary>
    [Column("publisher_ID")]
    public int PublisherId { get; set; }

    /// <summary>
    /// Publisher
    /// </summary>
    public virtual Publisher Publisher { get; set; } = null!;
}