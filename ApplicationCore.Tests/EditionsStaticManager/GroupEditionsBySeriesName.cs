using ApplicationCore.Tests.TestData;

namespace ApplicationCore.Tests.EditionsStaticManager;

/// <summary>
/// TESTS for the static method 
/// ApplicationCore.StaticManagers.EditionsStaticManager.GroupEditionsBySeriesName(IEnumerable<EditionResultDTO> editions)
/// </summary>
[TestClass]
public class GroupEditionsBySeriesName
{
    /// <summary>
    /// Test with the first set given into EditionsStaticManagerTestsData
    /// </summary>
    [TestMethod]
    public void WithDataSetOne()
    {
        var dataset = EditionsStaticManagerTestsData.FirstEditionsSet;

        var groupedEditions = StaticManagers.EditionsStaticManager.GroupEditionsBySeriesName(dataset);

        Assert.IsTrue(groupedEditions.Count == 3);

        Assert.IsTrue(groupedEditions.ContainsKey("0"));
        Assert.IsTrue(groupedEditions.ContainsKey("Narnia"));
        Assert.IsTrue(groupedEditions.ContainsKey("Pokemon"));

        Assert.IsTrue(groupedEditions["0"].Count == 6);
        Assert.IsTrue(groupedEditions["Narnia"].Count == 3);
        Assert.IsTrue(groupedEditions["Pokemon"].Count == 2);

        Assert.AreEqual("2 4 6 7 8 10", string.Join(' ', groupedEditions["0"].Select(ed => ed.Id).Order()));
        Assert.AreEqual("1 3 5", string.Join(' ', groupedEditions["Narnia"].Select(ed => ed.Id).Order()));
        Assert.AreEqual("9 11", string.Join(' ', groupedEditions["Pokemon"].Select(ed => ed.Id).Order()));
    }
}