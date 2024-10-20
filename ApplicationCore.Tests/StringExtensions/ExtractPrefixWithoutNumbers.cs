using ApplicationCore.Extensions;

namespace ApplicationCore.Tests.StringExtensions;

/// <summary>
/// TESTS for the static method 
/// ApplicationCore.Extensions.StringExtensions.ExtractPrefix(this string input)
/// </summary>
[TestClass]
public class ExtractPrefixWithoutNumbers
{
    /// <summary>
    /// If the given word starts with letters, these letters will be returned
    /// </summary>
    [TestMethod]
    public void TheWordBeginsWithLetters()
    {
        string word = "AL1546sjfzeuhfgb";

        string prefix = word.ExtractPrefixWithoutNumbers();

        Assert.AreEqual("AL", prefix);
    }

    /// <summary>
    /// If the given word starts with special characters, 
    /// these characters will be returned as if they were letters
    /// </summary>
    [TestMethod]
    public void TheWordBeginsWithSpecialCharacters()
    {
        string word = "&@à!156";

        string prefix = word.ExtractPrefixWithoutNumbers();

        Assert.AreEqual("&@à!", prefix);
    }

    /// <summary>
    /// If the given word starts with numbers, string.Empty will be returned
    /// </summary>
    [TestMethod]
    public void TheWordBeginsWithNumbers()
    {
        string word = "154sdfghdbfi";

        string prefix = word.ExtractPrefixWithoutNumbers();

        Assert.AreEqual(string.Empty, prefix);
    }
}