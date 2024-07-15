using System.Xml;
using System.Xml.Linq;
using mediatheque_back_csharp.Classes;
using mediatheque_back_csharp.Constants;
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
    /// Namespace "mxc"
    /// </summary>
    private XNamespace _nMxc = "info:lc/xmlns/marcxchange-v2";

    /// <summary>
    /// Namespace "srw"
    /// </summary>
    private XNamespace _nSrw = "http://www.loc.gov/zing/srw/";

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
    /// Constructs objects of type SearchResultDTO from XElements of the XML content
    /// </summary>
    /// <param name="xmlNodes">XML nodes</param>
    /// <returns>Final list of SearchResultDTOs</returns>
    private List<SearchResultDTO> ExtractResultsFromXmlNodes(ref IEnumerable<XElement> xmlNodes) {
        
        IEnumerable<BnfDataField> datafields;
        string isbn;

        /* Will contain objects whose keys are into the format "AUTHORNAME_BOOKNAME"
        ** and the values are dictionaries for editions data */
        List<KeyValuePair<string, Dictionary<string, string>>> results = new();

        foreach (var result in xmlNodes) {
            
            // Gets the datafields with the needed tags
            datafields = result.Descendants(_nMxc + "datafield").Where(node => 
                node.Attributes().ToList().Exists(
                    at => at.Name == "tag"
                    && BnfConsts.NEEDED_TAGS.Contains(at.Value)
                )
            ).Select(rawDataField => new BnfDataField() {
                Tag = rawDataField.Attribute("tag").Value,
                Ind1 = rawDataField.Attribute("ind1").Value,
                Ind2 = rawDataField.Attribute("ind2").Value,
                Subfields = rawDataField.Descendants().Select(rawSubfield => 
                    new BnfSubField {
                        Code = rawSubfield.Attribute("code").Value,
                        Value = rawSubfield.Value
                    }
                )
            });

            // Keep the result only if the ISBN is not empty
            if (!string.IsNullOrEmpty(isbn = SearchForValue(ref datafields, BnfPropertiesConsts.ISBN))) {
                results.Add(
                    new(
                        $"{SearchForValue(ref datafields, BnfPropertiesConsts.TITLE)};;;{SearchForValue(ref datafields, BnfPropertiesConsts.AUTHOR)}",
                        new() {
                            { BnfPropertiesConsts.ISBN, isbn },
                            { BnfPropertiesConsts.PUBLICATION_DATE_BNF, SearchForValue(ref datafields, BnfPropertiesConsts.PUBLICATION_DATE_BNF) },
                            { BnfPropertiesConsts.PUBLISHER, SearchForValue(ref datafields, BnfPropertiesConsts.PUBLISHER) },
                            { BnfPropertiesConsts.SERIES_NAME, SearchForValue(ref datafields, BnfPropertiesConsts.SERIES_NAME) },
                            { BnfPropertiesConsts.SUBTITLE, SearchForValue(ref datafields, BnfPropertiesConsts.SUBTITLE) },
                            { BnfPropertiesConsts.SUMMARY, SearchForValue(ref datafields, BnfPropertiesConsts.SUMMARY) },
                            { BnfPropertiesConsts.VOLUME, SearchForValue(ref datafields, BnfPropertiesConsts.VOLUME) }
                        }
                    )
                );
            }
        }

        var test = results[0].Value["isbn"];

        // var editionResultDto = new EditionResultDTO {
        //     Isbn = resultCustomObject.Isbn,
        //     Subtitle = resultCustomObject.Subtitle,
        //     PublicationYear = resultCustomObject.PublicationDate,
        //     Volume = resultCustomObject.Volume,
        //     Summary = resultCustomObject.Summary,
        //     Series = new SeriesResultDTO {
        //         SeriesName = resultCustomObject.SeriesName
        //     },
        //     Publisher = new PublisherResultDTO {
        //         PublishingHouse = resultCustomObject.Publisher
        //     }
        // };

        return new List<SearchResultDTO>();
    }

    /// <summary>
    /// Searches for a value into the datafields according to the property name
    /// </summary>
    /// <param name="datafields">List of filtered BnfDataField objects</param>
    /// <param name="propertyName">Name of the property, refers to the file BnfConsts</param>
    /// <returns>A string value</returns>
    private string SearchForValue(ref IEnumerable<BnfDataField> datafields, string propertyName) {

        BnfDataField? extractedDataf;
        string? extractedValue = string.Empty;

        // Gets the convenient tags and codes for the given property name
        var searchedTagsAndCodes = BnfConsts.TAGS_AND_CODES[propertyName];

        /* "searchParams" contains a tag, ind1, ind2 and the bound codes */
        foreach (var searchParams in searchedTagsAndCodes) {

            // Takes the BnfDataField corresponding to the tag
            extractedDataf = datafields.FirstOrDefault(
                dataf => dataf.Equals(searchParams.Key)
            );
            
            if (extractedDataf == null) {
                continue;
            }

            // Searches for the value into the subfields whose codes are given
            extractedValue = searchParams.Value.Select(code =>
                extractedDataf.ExtractValueFromSubfield(code)
            ).FirstOrDefault(v => 
                !string.IsNullOrEmpty(v)
            );

            if (!string.IsNullOrEmpty(extractedValue)) {
                return extractedValue;
            }
        }

        return extractedValue ?? string.Empty;
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

        // For a simple search, the criterion should be a string value !
        if (criterion.GetType() != typeof(string)) {
            throw new ArgumentException(TextConsts.BNF_SEARCH_SERVICE_ERROR_CRITERION_TYPE);
        }

        var outResults = new List<SearchResultDTO>();
        var stringCriterion = (string)criterion;
        var url = BnfConsts.BNF_API_URL_BASE + GetSimpleSearchConditions(stringCriterion, 20);

        // Configures the XML reader
        var readerSettings = new XmlReaderSettings() {
            Async = true
        };

        using (var reader = XmlReader.Create(url, readerSettings)) {

            // Loads the XML content from the url
            var fileRoot = await XDocument.LoadAsync(
                reader, 
                LoadOptions.None, 
                CancellationToken.None
            );

            // Checks the number of records
            var recordsNumberNode = fileRoot.Descendants(_nSrw + "numberOfRecords")
                                            .FirstOrDefault();

            if (!int.TryParse(recordsNumberNode?.Value, out int resultsNumber) || resultsNumber <= 0) {
                return new List<SearchResultDTO>();
            }

            // Checks the results nodes
            var resultsNodes = fileRoot.Descendants(_nMxc + "record");

            if (resultsNodes == null || resultsNodes.Count() <= 0) {
                return new List<SearchResultDTO>();
            }

            outResults = ExtractResultsFromXmlNodes(ref resultsNodes);
        }

        return outResults;
    }
}