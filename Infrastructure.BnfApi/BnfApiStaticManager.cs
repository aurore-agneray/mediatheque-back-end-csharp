using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Infrastructure.BnfApi;

/// <summary>
/// Gives static methods for preparing the request sent to the BnF
/// </summary>
public class BnfApiStaticManager
{
    protected const string DOUBLE_QUOTES = "\"";
    protected const string SPACE = " ";
    protected const string PLUS = "+";
    protected const string NUMBER_OF_RECORDS = "numberOfRecords";
    protected const string RECORD = "record";

    /// <summary>
    /// Namespace "mxc"
    /// </summary>
    protected static readonly XNamespace _nMxc = "info:lc/xmlns/marcxchange-v2";

    /// <summary>
    /// Namespace "srw"
    /// </summary>
    protected static readonly XNamespace _nSrw = "http://www.loc.gov/zing/srw/";

    /// <summary>
    /// Constructs the complete string with the conditions to
    /// send to the BnF API
    /// </summary>
    /// <param name="criterion">Criterion entered by the user to launch the searching process</param>
    /// <param name="noticesQty">The quantity of notices returned by the API</param>
    /// <returns>Returns the conditions part of the URL for the request</returns>
    public static string GetSimpleSearchConditions(string criterion, int noticesQty)
    {
        var stringBuilder = new StringBuilder();

        if (string.IsNullOrEmpty(criterion))
        {
            return string.Empty;
        }

        if (noticesQty < BnfGlobalConsts.DEFAULT_NOTICES_NUMBER)
        {
            noticesQty = BnfGlobalConsts.DEFAULT_NOTICES_NUMBER;
        }

        stringBuilder = stringBuilder.Clear();

#pragma warning disable CA1834
        /* The compiler considers DOUBLE_QUOTES as a unit char, even if it's not the case !
        ** https://learn.microsoft.com/fr-fr/dotnet/fundamentals/code-analysis/quality-rules/ca1834 */
        stringBuilder.Append(DOUBLE_QUOTES)
                     .Append(criterion.Replace(SPACE, PLUS))
                     .Append(DOUBLE_QUOTES);
#pragma warning restore CA1834

        criterion = stringBuilder.ToString();

        return BnfGlobalConsts.SIMPLE_SEARCH_PARAMETERED_CONDITIONS(criterion, noticesQty);
    }

    /// <summary>
    /// Returns all the XML data from the BnF API and the given request url
    /// </summary>
    /// <param name="url">URL constructed with the criteria given by the user</param>
    /// <returns>A XDocument object that contains all the data returned by the BnF</returns>
    public static async Task<XDocument> CallBnfApi(string url)
    {
        // Configures the XML reader
        var readerSettings = new XmlReaderSettings()
        {
            Async = true
        };

        XDocument bnfData;

        using (var reader = XmlReader.Create(url, readerSettings))
        {
            // Loads the XML content from the url
            bnfData = await XDocument.LoadAsync(
                reader,
                LoadOptions.None,
                CancellationToken.None
            );
        }

        return bnfData;
    }

    /// <summary>
    /// Returns XML results nodes from the BnF API and the given request url
    /// </summary>
    /// <param name="bnfData">A XDocument object that contains all data returned by the BnF</param>
    /// <returns>An enumerable of XElement objects that are "records"</returns>
    public static IEnumerable<XElement> ExtractXMLResultsNodes(XDocument bnfData)
    {
        if (bnfData == null)
        {
            return [];
        }

        // Checks the number of records
        var recordsNumberNode = bnfData.Descendants(_nSrw + NUMBER_OF_RECORDS)
                                        .FirstOrDefault();

        if (!int.TryParse(recordsNumberNode?.Value, out int resultsNumber) || resultsNumber <= 0)
        {
            return [];
        }

        // Extracts the results nodes
        return bnfData.Descendants(_nMxc + RECORD);
    }
}