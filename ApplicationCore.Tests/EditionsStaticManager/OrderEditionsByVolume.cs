using ApplicationCore.Tests.TestData;

namespace ApplicationCore.Tests.EditionsStaticManager;

/// <summary>
/// TESTS for the static method 
/// ApplicationCore.StaticManagers.EditionsStaticManager.OrderEditionsByVolume(IEnumerable<EditionResultDTO> editions)
/// </summary>
[TestClass]
public class OrderEditionsByVolume
{
    /// <summary>
    /// Test with the first set given into EditionsManagerTestsData
    /// </summary>
    [TestMethod]
    public void WithDataSetOne()
    {
        var dataset = EditionsStaticManagerTestsData.FirstEditionsSet;

        var orderedEditions = StaticManagers.EditionsStaticManager.OrderEditionsByVolume(dataset);

        Assert.IsTrue(orderedEditions.Count == 11);

        Assert.IsTrue(orderedEditions[0].Id == 6);
        Assert.IsTrue(orderedEditions[1].Id == 4);
        Assert.IsTrue(orderedEditions[2].Id == 7);
        Assert.IsTrue(orderedEditions[3].Id == 2);
        Assert.IsTrue(orderedEditions[4].Id == 5);
        Assert.IsTrue(orderedEditions[5].Id == 10);
        Assert.IsTrue(orderedEditions[6].Id == 9);
        Assert.IsTrue(orderedEditions[7].Id == 3);
        Assert.IsTrue(orderedEditions[8].Id == 1);
        Assert.IsTrue(orderedEditions[9].Id == 11);
        Assert.IsTrue(orderedEditions[10].Id == 8);
    }
}