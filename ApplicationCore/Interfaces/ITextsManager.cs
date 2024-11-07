namespace ApplicationCore.Interfaces;

/// <summary>
/// Describes the behavior of a TextsManager
/// </summary>
public interface ITextsManager {

    /// <summary>
    /// Get a specific text from the texts resources
    /// </summary>
    /// <param name="key">Key name of the wanted text</param>
    /// <returns>A string value</returns>
    public string GetText(string key);
}