using ApplicationCore.Interfaces.Entities;

namespace ApplicationCore.Dtos;

/// <summary>
/// Object retrieved for the INamed entities while loading the app
/// </summary>
public class NamedDTO : INamed
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = null!;
}