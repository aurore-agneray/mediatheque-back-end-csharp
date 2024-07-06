using mediatheque_back_csharp.Interfaces;

namespace mediatheque_back_csharp.Dtos;

/// <summary>
/// DTO for the Genres
/// </summary>
public class GenreDTO : IIdentified
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = null!;
}