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
    /// Indicates if it is an advanced or a simple search
    /// </summary>
    public bool IsAdvanced { get; set; }

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

        return string.Concat(
            $"bib.author all {criterion} or bib.title all {criterion} ",
            $"or bib.isbn all {criterion} or bib.serialtitle all {criterion} ",
            $"&recordSchema=unimarcxchange&maximumRecords={noticesQty}&startRecord=1"
        );
    }

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
            throw new ArgumentException("The criterion should be a string but it is not the case !");
        }

        var stringCriterion = (string)criterion;

        var url = "http://catalogue.bnf.fr/api/SRU?version=1.2&operation=searchRetrieve&query=";
        url += GetSimpleSearchConditions(stringCriterion, 20);

        var readerSettings = new XmlReaderSettings() {
            Async = true
        };

        using (var reader = XmlReader.Create(url, readerSettings)) {
            
            var content = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None);
            var descendants = content.Descendants();
            var test = descendants.Nodes();
        }

        return new List<SearchResultDTO>();
    }
}