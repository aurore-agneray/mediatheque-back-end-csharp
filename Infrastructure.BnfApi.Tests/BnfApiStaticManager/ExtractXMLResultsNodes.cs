using System.Xml;
using System.Xml.Linq;

namespace Infrastructure.BnfApi.Tests.BnfApiStaticManager;

[TestClass]
public class ExtractXMLResultsNodes
{
    private const string _testsDataCompleteFilePath = @"D:\PROGRAMMATION\Projets C#\0 - MES APPLIS\MediathequeBackCSharp\Infrastructure.BnfApi.Tests\XMLTestsData\";

    /// <summary>
    /// Reads the data from a file added into the subfolder XMLTestsData
    /// </summary>
    /// <param name="fileName">File name (with extension)</param>
    private async Task<XDocument> ReadTestFile(string fileName)
    {
        // Configures the XML reader
        var readerSettings = new XmlReaderSettings()
        {
            Async = true
        };

        XDocument testData;

        using (var reader = XmlReader.Create(Path.Combine(_testsDataCompleteFilePath + fileName), readerSettings))
        {
            // Loads the XML content from the url
            testData = await XDocument.LoadAsync(
                reader,
                LoadOptions.None,
                CancellationToken.None
            );
        }

        return testData;
    }

    private string ExtractArkId(XElement element)
    {
        if (element?.Name == BnfGlobalConsts.NAMESPACE_MXC + "record" && element?.Attribute("id") != null)
        {
#pragma warning disable CS8602
            return element.Attribute("id").Value;
#pragma warning restore CS8602
        }

        return string.Empty;
    }

    [TestMethod]
    public async Task NodesContainResults()
    {
        /* The given document contains 4 results identified by their ArkId */
        XDocument dataSource = await ReadTestFile("ResponseExampleWithResults.xml");

        var results = BnfApi.BnfApiStaticManager.ExtractXMLResultsNodes(dataSource);

        Assert.IsNotNull(results);
        Assert.AreEqual(4, results.Count());
        Assert.AreEqual("ark:/12148/cb43393050j", ExtractArkId(results.ElementAt(0)));
        Assert.AreEqual("ark:/12148/cb43393051w", ExtractArkId(results.ElementAt(1)));
        Assert.AreEqual("ark:/12148/cb400693581", ExtractArkId(results.ElementAt(2)));
        Assert.AreEqual("ark:/12148/cb43488086q", ExtractArkId(results.ElementAt(3)));
    }

    [TestMethod]
    public async Task NodesDontContainResults()
    {
        /* The given document contains no results */
        XDocument dataSource = await ReadTestFile("ResponseExampleWithoutResults.xml");

        var results = BnfApi.BnfApiStaticManager.ExtractXMLResultsNodes(dataSource);

        Assert.IsNotNull(results);
        Assert.AreEqual(0, results.Count());
    }
}