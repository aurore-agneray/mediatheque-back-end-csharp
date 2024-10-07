namespace ApplicationCore.Extensions;

/// <summary>
/// Describes extensions for the basic class Dictionary<TKey, TValue>
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Check into the string dictionary if the given key exists,
    /// and returns the associated value, or an empty string
    /// </summary>
    /// <param name="dict">Dictionary with keys and values of the type "string"</param>
    /// <param name="propertyName">Name of the retrieved property's value</param>
    /// <returns>Returns a string value, or an empty string</returns>
    public static string GetValueOrEmptyString(this Dictionary<string, string> dict, string key)
    {
        if (dict.TryGetValue(key, out string? value)) {
            return value;
        }
        
        return string.Empty;
    }
}