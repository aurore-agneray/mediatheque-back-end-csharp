using ApplicationCore.Extensions;

namespace ApplicationCore.Tests.StringExtensions;

/// <summary>
/// TESTS for the static method 
/// ApplicationCore.Extensions.StringExtensions.ExtractNumber(this string input)
/// </summary>
[TestClass]
public class ExtractNumber
{
    /// <summary>
    /// If the word has no number, the method returns 0
    /// </summary>
    [TestMethod]
    public void TheWordHasNoNumber()
    {
        string word = "Bonjour !";

        int extractedNumber = word.ExtractNumber();

        Assert.AreEqual(0, extractedNumber);
    }

    /// <summary>
    /// If the word has one number, the method returns this number
    /// </summary>
    [TestMethod]
    public void TheWordHasOneNumber()
    {
        string word = "Bonjour 1566 ! ça va ?";

        int extractedNumber = word.ExtractNumber();

        Assert.AreEqual(1566, extractedNumber);
    }

    /// <summary>
    /// If the word has several numbers, the method returns the first one
    /// </summary>
    [TestMethod]
    public void TheWordHasSeveralNumbers()
    {
        string word = "Bonjour 45896 ! ça va ? Moi je suis 564 ! Et lui c'est 4875.";

        int extractedNumber = word.ExtractNumber();

        Assert.AreEqual(45896, extractedNumber);
    }
}