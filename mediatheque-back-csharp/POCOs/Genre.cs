using ApplicationCore.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace mediatheque_back_csharp.Pocos;

/// <summary>
/// POCO for the Genres
/// </summary>
[Table("Genre")]
public class Genre : INamed
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    [Column("ID")]
    public int Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [Column("genre_name")]
    public string Name { get; set; } = null!;
}