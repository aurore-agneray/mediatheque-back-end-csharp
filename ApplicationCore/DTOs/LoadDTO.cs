namespace ApplicationCore.Dtos;

/// <summary>
/// Contains the data loaded when the user opens the front-end application
/// </summary>
public class LoadDTO
{
    /// <summary>
    /// All Genres of the database
    /// </summary>
    public List<NamedDTO> Genres { get; set; } = [];

    /// <summary>
    /// All Publishers of the database
    /// </summary>
    public List<NamedDTO> Publishers { get; set; } = [];

    /// <summary>
    /// All Formats of the database
    /// </summary>
    public List<NamedDTO> Formats { get; set; } = [];
}