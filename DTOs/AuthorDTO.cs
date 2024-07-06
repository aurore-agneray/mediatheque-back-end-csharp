using mediatheque_back_csharp.Interfaces;

namespace mediatheque_back_csharp.Dtos;

/// <summary>
/// DTO for the Authors
/// </summary>
public class AuthorDTO : IIdentified
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Custom code
    /// </summary>
    public String Code { get; set; } = null!;

    /// <summary>
    /// First name
    /// </summary>
    public String? FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public String? LastName { get; set; }

    /// <summary>
    /// First name + last name (generated into the database)
    /// </summary>
    public String? CompleteName { get; set; }
}