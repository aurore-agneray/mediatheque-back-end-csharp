using System.Reflection;
using System.Resources;

namespace MediathequeBackCSharp.Texts;

/// <summary>
/// Defines a singleton used for reading the string content into the texts resources file
/// </summary>
public sealed class TextsManager
{
    /// <summary>
    /// Unique instance of the TextsManager
    /// </summary>
    private static ResourceManager _instance = null!;

    /// <summary>
    /// A necessary locker for multi-threading contexts
    /// </summary>
    private static readonly object _padlock = new();

    /// <summary>
    /// Main constructor which is private in order to forbidden the creation of an instance
    /// </summary>
    private TextsManager() {}

    /// <summary>
    /// Permits to return the instance
    /// </summary>
    public static ResourceManager Instance {
        get
        {
            lock (_padlock)
            {
                if (_instance == null)
                {
                    CreateInstance();
                }

                return _instance!;
            }
        }
    }

    private static void CreateInstance()
    {
        _instance = new ResourceManager(@"MediathequeBackCSharp.Texts.FrTexts", Assembly.GetExecutingAssembly());
    }
}