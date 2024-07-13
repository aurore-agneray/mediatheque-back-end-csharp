using System.Xml;
using System.Xml.Linq;
using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Interfaces;

namespace mediatheque_back_csharp.Services;

/// <summary>
/// Methods for preparing the data retrieved from the Bnf's API
/// </summary>
public class BnfSearchService : ISearchService
{
    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="isAdvanced">Indicates if it is an advanced or a simple search</param>
    public BnfSearchService(bool isAdvanced)
    {
        IsAdvanced = isAdvanced;
    }

    /// <summary>
    /// Constructs the complete string with the conditions to
    /// send to the BnF API
    /// </summary>
    /// <param name="criterion">Criterion entered by the user to launch the searching process</param>
    /// <param name="noticesQty">The quantity of notices returned by the API</param>
    /// <returns>Returns the conditions part of the URL for the request</returns>
    private string GetSimpleSearchConditions(string criterion, int noticesQty) {

        if (string.IsNullOrEmpty(criterion)) {
            return string.Empty;
        }

        if (noticesQty < 20) {
            noticesQty = 20;
        }

        criterion = "\"" + criterion.Replace(" ", "+") + "\"";

        return BnfConsts.SIMPLE_SEARCH_PARAMETERED_CONDITIONS(criterion, noticesQty);
    }

     /// <summary>
    /// Indicates if it is an advanced or a simple search
    /// </summary>
    public bool IsAdvanced { get; set; }

    /// <summary>
    /// Retrieves and returns the results of the wanted search
    /// </summary>
    /// <param name="criterion">
    /// The type of that property depends on the type of
    /// search. For the BnF search it will be a string.
    /// For the MySQL database it will be an array to send to the ORM.
    /// </param>
    /// <returns>A list of SearchResultDTOs</returns>
    public async Task<IEnumerable<SearchResultDTO>> GetResults(object criterion) {

        if (criterion.GetType() != typeof(string)) {
            throw new ArgumentException(TextConsts.BNF_SEARCH_SERVICE_ERROR_CRITERION_TYPE);
        }

        var stringCriterion = (string)criterion;

        var url = BnfConsts.BNF_API_URL_BASE;
        url += GetSimpleSearchConditions(stringCriterion, 20);

        var readerSettings = new XmlReaderSettings() {
            Async = true
        };

        using (var reader = XmlReader.Create(url, readerSettings)) {
            
            var fileRoot = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None);
            var mainNodes = fileRoot.Descendants();
            var recordsNumberNode = mainNodes?.FirstOrDefault(no => no.Name.LocalName == "numberOfRecords");
            var resultsNumberStr = string.Empty;
            var resultsNumber = -1;

            if ((resultsNumberStr = recordsNumberNode?.FirstNode?.ToString()) == null
                || (resultsNumber = int.Parse(resultsNumberStr)) <= 0) {
                return new List<SearchResultDTO>();
            }

            /* Gets the "mxc:record" nodes
            ** The namespace for "mxc" is "info:lc/xmlns/marcxchange-v2"
            ** and the one for "srw" is "http://www.loc.gov/zing/srw/"
            */ 
            var resultsNodes = mainNodes?.FirstOrDefault(no => no.Name.LocalName == "records")
                                        ?.Descendants()
                                        ?.Where(node => node.Name.NamespaceName == "info:lc/xmlns/marcxchange-v2");

            if (resultsNodes == null || resultsNodes.Count() <= 0) {
                return new List<SearchResultDTO>();
            }
        }

        return new List<SearchResultDTO>();
    }
}