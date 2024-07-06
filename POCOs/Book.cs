using mediatheque_back_csharp.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace mediatheque_back_csharp.Pocos;

/// <summary>
/// POCO for the Books
/// </summary>
[Table("Book")]
public class Book : IIdentified
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    [Column("ID")]
    public int Id { get; set; }

    /// <summary>
    /// Custom code
    /// </summary>
    [Column("book_code")]
    public string BookCode { get; set; } = null!;

    /// <summary>
    /// Title
    /// </summary>
    [Column("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Staff comment
    /// </summary>
    [Column("staff_comment")]
    public string? StaffComment { get; set; }

    /// <summary>
    /// Author ID
    /// </summary>
    [Column("auth_ID")]
    public int? AuthorId { get; set; }

    /// <summary>
    /// Genre ID
    /// </summary>
    [Column("genre_ID")]
    public int? GenreId { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    public virtual Author? Author { get; set; }

    /// <summary>
    /// Genre
    /// </summary>
    public virtual Genre? Genre { get; set; }

    /// <summary>
    /// List of editions
    /// </summary>
    public virtual ICollection<Edition> Editions { get; set; } = new List<Edition>();
}