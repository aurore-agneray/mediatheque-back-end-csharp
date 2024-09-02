namespace ApplicationCore.Interfaces.DTOs;

/// <summary>
/// Arguments received for a simple search
/// </summary>
public interface ISimpleSearchDTO : ISearchDTO
{
    /// <summary>
    /// Criterion for the simple search (Book's title, author's name, series' name or ISBN)
    /// </summary>
    public string? SimpleCriterion { get; set; }
}