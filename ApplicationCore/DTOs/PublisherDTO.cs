using ApplicationCore.Interfaces.Entities;

namespace ApplicationCore.Dtos;

/// <summary>
/// DTO for the Publishers
/// </summary>
public class PublisherDTO : IIdentified
{
    /// <summary>
    /// ID (primary key)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Custom code
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Publishing house name
    /// </summary>
    public string? PublishingHouse { get; set; }

    /// <summary>
    /// Country name
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Year of establishment
    /// </summary>
    public int? YearEstablished { get; set; }
}