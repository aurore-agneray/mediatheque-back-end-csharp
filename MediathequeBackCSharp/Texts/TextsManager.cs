using System.Reflection;
using System.Resources;
using ApplicationCore.Interfaces;

namespace MediathequeBackCSharp.Texts;

/// <summary>
/// Defines a singleton used for reading the string content into the texts resources file
/// </summary>
public class TextsManager : ITextsManager
{
    /// <summary>
    /// Contains the texts resources
    /// </summary>
    private ResourceManager _resources = null!;

    /// <summary>
    /// Main constructor
    /// </summary>
    public TextsManager()
    {
        _resources = new ResourceManager(@"MediathequeBackCSharp.Texts.FrTexts", Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Get a specific text from the texts resources
    /// </summary>
    /// <param name="key">Key name of the wanted text</param>
    /// <returns>A string value</returns>
    public string GetText(string key)
    {
        return _resources.GetString(key) ?? string.Empty;
    }
}