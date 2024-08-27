using ApplicationCore.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Pocos;

/// <summary>
/// POCO for the Formats
/// </summary>
[Table("Format")]
public class Format : INamed
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    [Column("ID")]
    public int Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [Column("format_name")]
    public string Name { get; set; } = null!;
}