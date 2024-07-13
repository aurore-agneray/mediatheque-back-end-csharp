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
    /// Retrieves and returns the results of the wanted search
    /// </summary>
    /// <param name="criterion">
    /// The type of that property depends on the type of
    /// search. For the BnF search it will be a string.
    /// For the MySQL database it will be an array to send to the ORM.
    /// </param>
    /// <returns>A list of SearchResultDTOs</returns>
    public async Task<IEnumerable<SearchResultDTO>> GetResults(object criterion) {

        var url = "http://catalogue.bnf.fr/api/SRU?version=1.2&operation=searchRetrieve&query=bib.author%20all%20%22narnia%22%20and%20bib.title%20all%20%22narnia%22&recordSchema=unimarcxchange&maximumRecords=20&startRecord=1";

        var readerSettings = new XmlReaderSettings() {
            Async = true
        };

        using (var reader = XmlReader.Create(url, readerSettings)) {
            
            var content = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None);
        }

        return new List<SearchResultDTO>();
    }
}