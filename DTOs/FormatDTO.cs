using mediatheque_back_csharp.Interfaces;

namespace mediatheque_back_csharp.Dtos;

/// <summary>
/// DTO for the Formats
/// </summary>
public class FormatDTO : IIdentified
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