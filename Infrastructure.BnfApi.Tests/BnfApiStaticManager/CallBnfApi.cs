using System.Xml.Linq;

namespace Infrastructure.BnfApi.Tests.BnfApiStaticManager;

[TestClass]
public class CallBnfApi
{
    [TestMethod]
    public async Task GetAResponseWithResultsNodes()
    {
        var url = BnfApi.BnfApiStaticManager.GetCompleteUrl("narnia", 20);

        XDocument bnfData = await BnfApi.BnfApiStaticManager.CallBnfApi(url);
        var recordsNumberNode = bnfData.Descendants(BnfGlobalConsts.NAMESPACE_SRW + "numberOfRecords").FirstOrDefault();
        var recordsNumberIsGreaterThanZero = int.TryParse(recordsNumberNode?.Value, out int resultsNumber) && resultsNumber > 0;
        var allRecordsSuperNode = bnfData.Descendants(BnfGlobalConsts.NAMESPACE_SRW + "records").FirstOrDefault();
        var allRecordsNodes = bnfData.Descendants(BnfGlobalConsts.NAMESPACE_SRW + "record");

        Assert.IsNotNull(bnfData?.Root?.Name);
        Assert.AreEqual(BnfGlobalConsts.NAMESPACE_SRW + "searchRetrieveResponse", bnfData.Root.Name.ToString());
        Assert.IsTrue(recordsNumberIsGreaterThanZero);
        Assert.IsNotNull(allRecordsSuperNode);
        Assert.IsNotNull(allRecordsNodes);
        Assert.IsTrue(allRecordsNodes.Any());
    }

    [TestMethod]
    public async Task GetAResponseWithoutResultsNodes()
    {
        var url = BnfApi.BnfApiStaticManager.GetCompleteUrl("zehfgbzuebf", 200);

        XDocument bnfData = await BnfApi.BnfApiStaticManager.CallBnfApi(url);
        var recordsNumberNode = bnfData.Descendants(BnfGlobalConsts.NAMESPACE_SRW + "numberOfRecords").FirstOrDefault();
        var recordsNumberEqualsZero = int.TryParse(recordsNumberNode?.Value, out int resultsNumber) && resultsNumber == 0;
        var allRecordsSuperNode = bnfData.Descendants(BnfGlobalConsts.NAMESPACE_SRW + "records").FirstOrDefault();
        var allRecordsNodes = bnfData.Descendants(BnfGlobalConsts.NAMESPACE_SRW + "record");

        Assert.IsNotNull(bnfData?.Root?.Name);
        Assert.AreEqual(BnfGlobalConsts.NAMESPACE_SRW + "searchRetrieveResponse", bnfData.Root.Name.ToString());
        Assert.IsTrue(recordsNumberEqualsZero);
        Assert.IsNotNull(allRecordsSuperNode);
        Assert.IsNotNull(allRecordsNodes);
        Assert.IsTrue(!allRecordsNodes.Any());
    }
}