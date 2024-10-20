namespace ApplicationCore.Tests.TestData;

/// <summary>
/// Data sets for testing the static methods from DictionaryExtensions class
/// </summary>
internal static class DictionaryExtensionsTestsData
{
    internal static Dictionary<string, string> FirstDico => new Dictionary<string, string>
    {
        { "id", "1" },
        { "name", "Aurore" },
        { "city", "Calais" }
    };

    internal static Dictionary<string, string> SecondDico => new Dictionary<string, string>
    {
        { "id", "6" },
        { "title", "L'attaque des titans" },
        { "edition", "Kana" }
    };
}