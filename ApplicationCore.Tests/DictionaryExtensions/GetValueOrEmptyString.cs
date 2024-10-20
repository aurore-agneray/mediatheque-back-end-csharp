using ApplicationCore.Extensions;
using ApplicationCore.Tests.TestData;

namespace ApplicationCore.Tests.DictionaryExtensions;

/// <summary>
/// TESTS for the static method 
/// ApplicationCore.Extensions.DictionaryExtensions.GetValueOrEmptyString(this Dictionary<string, string> dict, string key)
/// </summary>
[TestClass]
public class GetValueOrEmptyString
{
    private readonly Dictionary<string, string> _firstDico;
    private readonly Dictionary<string, string> _secondDico;

    /// <summary>
    /// Constructor for initializing common test dictionaries
    /// </summary>
    public GetValueOrEmptyString()
    {
        _firstDico = DictionaryExtensionsTestsData.FirstDico;
        _secondDico = DictionaryExtensionsTestsData.SecondDico;
    }

    [TestMethod]
    public void GetValuesWithIdKey()
    {
        var key = "id";

        var valueFrom1 = _firstDico.GetValueOrEmptyString(key);
        var valueFrom2 = _secondDico.GetValueOrEmptyString(key);

        Assert.IsTrue(_firstDico.ContainsKey(key));
        Assert.IsTrue(_secondDico.ContainsKey(key));
        Assert.AreEqual(valueFrom1, "1");
        Assert.AreEqual(valueFrom2, "6");
    }

    [TestMethod]
    public void GetValuesWithNameKey()
    {
        var key = "name";

        var valueFrom1 = _firstDico.GetValueOrEmptyString(key);
        var valueFrom2 = _secondDico.GetValueOrEmptyString(key);

        Assert.IsTrue(_firstDico.ContainsKey(key));
        Assert.IsFalse(_secondDico.ContainsKey(key));
        Assert.AreEqual(valueFrom1, "Aurore");
        Assert.AreEqual(valueFrom2, string.Empty);
    }

    [TestMethod]
    public void GetValuesWithCityKey()
    {
        var key = "city";

        var valueFrom1 = _firstDico.GetValueOrEmptyString(key);
        var valueFrom2 = _secondDico.GetValueOrEmptyString(key);

        Assert.IsTrue(_firstDico.ContainsKey(key));
        Assert.IsFalse(_secondDico.ContainsKey(key));
        Assert.AreEqual(valueFrom1, "Calais");
        Assert.AreEqual(valueFrom2, string.Empty);
    }

    [TestMethod]
    public void GetValuesWithTitleKey()
    {
        var key = "title";

        var valueFrom1 = _firstDico.GetValueOrEmptyString(key);
        var valueFrom2 = _secondDico.GetValueOrEmptyString(key);

        Assert.IsFalse(_firstDico.ContainsKey(key));
        Assert.IsTrue(_secondDico.ContainsKey(key));
        Assert.AreEqual(valueFrom1, string.Empty);
        Assert.AreEqual(valueFrom2, "L'attaque des titans");
    }

    [TestMethod]
    public void GetValuesWithEditionKey()
    {
        var key = "edition";

        var valueFrom1 = _firstDico.GetValueOrEmptyString(key);
        var valueFrom2 = _secondDico.GetValueOrEmptyString(key);

        Assert.IsFalse(_firstDico.ContainsKey(key));
        Assert.IsTrue(_secondDico.ContainsKey(key));
        Assert.AreEqual(valueFrom1, string.Empty);
        Assert.AreEqual(valueFrom2, "Kana");
    }
}