using ApplicationCore.Interfaces;

namespace ApplicationCore.Dtos;

/// <summary>
/// DTO for the Books
/// </summary>
public class BookDTO : IIdentified
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Staff comment
    /// </summary>
    public string? StaffComment { get; set; }

    /// <summary>
    /// Author DTO
    /// </summary>
    public AuthorDTO? Author { get; set; }

    /// <summary>
    /// Genre DTO
    /// </summary>
    public NamedDTO? Genre { get; set; }
}