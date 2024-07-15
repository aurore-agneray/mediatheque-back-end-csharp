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
    /// Used for separating the title and the author's name
    /// when they are concatenated
    /// </summary>
    private const string TITLE_AND_AUTHOR_NAME_SEPARATOR = ";;;";

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
    /// Extracts the DataField nodes' data from the given XElement
    /// which corresponds to an edition
    /// </summary>
    /// <param name="result">Object of type XElement</param>
    /// <returns>A list of BnfDataFields</returns>
    private IEnumerable<BnfDataField> GetDataFieldsFromXElement(XElement result) {

        return result.Descendants(_nMxc + "datafield")
            .Where(node => 
                node.Attributes().ToList().Exists(
                    at => at.Name == "tag"
                    && BnfConsts.NEEDED_TAGS.Contains(at.Value)
                )
            )
            .Select(rawDataField => new BnfDataField() {
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
    }

    /// <summary>
    /// Extracts the properties of the edition from the given BnfDataField list.
    /// The names of these properties are defined into the class BnfPropertiesConsts 
    /// </summary>
    /// <param name="datafields">List of BnfDataField objects</param>
    /// <returns>An KeyValuePair whose key is into the format "AUTHORNAME_BOOKNAME" 
    /// and the value is a dictionary containing the edition's data</returns>
    private KeyValuePair<string, Dictionary<string, string>> ExtractOneEditionData(ref IEnumerable<BnfDataField> datafields) {
        
        string isbn;

        // Keep the result only if the ISBN is not empty
        if (!string.IsNullOrEmpty(isbn = SearchForValue(ref datafields, BnfPropertiesConsts.ISBN))) {
            return new(
                $"{SearchForValue(ref datafields, BnfPropertiesConsts.TITLE)}{TITLE_AND_AUTHOR_NAME_SEPARATOR}{SearchForValue(ref datafields, BnfPropertiesConsts.AUTHOR)}",
                new() {
                    { BnfPropertiesConsts.ISBN, isbn },
                    { BnfPropertiesConsts.PUBLICATION_DATE_BNF, SearchForValue(ref datafields, BnfPropertiesConsts.PUBLICATION_DATE_BNF) },
                    { BnfPropertiesConsts.PUBLISHER, SearchForValue(ref datafields, BnfPropertiesConsts.PUBLISHER) },
                    { BnfPropertiesConsts.SERIES_NAME, SearchForValue(ref datafields, BnfPropertiesConsts.SERIES_NAME) },
                    { BnfPropertiesConsts.SUBTITLE, SearchForValue(ref datafields, BnfPropertiesConsts.SUBTITLE) },
                    { BnfPropertiesConsts.SUMMARY, SearchForValue(ref datafields, BnfPropertiesConsts.SUMMARY) },
                    { BnfPropertiesConsts.VOLUME, SearchForValue(ref datafields, BnfPropertiesConsts.VOLUME) }
                }
            );
        }

        return default;
    }

    /// <summary>
    /// Constructs objects of type SearchResultDTO from XElements of the XML content
    /// </summary>
    /// <param name="xmlNodes">XML nodes</param>
    /// <returns>Final list of SearchResultDTOs</returns>
    private List<SearchResultDTO> ExtractResultsFromXmlNodes(ref IEnumerable<XElement> xmlNodes) {
        
        IEnumerable<BnfDataField> datafields;
        KeyValuePair<string, Dictionary<string, string>> extractedEditionData;
        string seriesName, title = null, authorName = null;
        var outList = new List<SearchResultDTO>();

        /* Will contain objects whose keys are into the format "AUTHORNAME_BOOKNAME"
        ** and the values are dictionaries for editions data */
        List<KeyValuePair<string, Dictionary<string, string>>> results = new();

        foreach (var result in xmlNodes) {
            
            datafields = GetDataFieldsFromXElement(result);
            extractedEditionData = ExtractOneEditionData(ref datafields);

            if (!extractedEditionData.Equals(default(KeyValuePair<string, Dictionary<string, string>>))) {
                results.Add(extractedEditionData);
            }
        }

        // Orders the results by book and author's name
        results = results.OrderBy(res => res.Key).ToList();

        // Groups the results by book and author's name
        var groupedByBooks = results.GroupBy(res => res.Key);

        // Creates the DTOs
        int bookId = 1;

        foreach (var book in groupedByBooks) {

            // Extracts the book's title and the author's name
            var titleAndAuthorArray = book.Key.Split(TITLE_AND_AUTHOR_NAME_SEPARATOR);

            if (titleAndAuthorArray.Count() < 2) {
                continue;
            }

            title = titleAndAuthorArray[0];
            authorName = titleAndAuthorArray[1];

            // Creates for each book a SearchResultDTO that will contain several editions
            var searchResultDto = new SearchResultDTO {
                BookId = bookId,
                Book = new BookResultDTO {
                    Title = title,
                    Author = new AuthorResultDTO {
                        CompleteName = authorName
                    }
                }
            };

            // Groups the concerned editions by the series' name
            var editionsGroupedBySeries = book.ToList().GroupBy(
                editionData => {
                    seriesName = editionData.Value[BnfPropertiesConsts.SERIES_NAME];

                    if (string.IsNullOrEmpty(seriesName)) {
                        return "0";
                    }
                    return seriesName;
                }
            );

            // Completes the list of the editions for the concerned book
            searchResultDto.Editions = editionsGroupedBySeries.ToDictionary(
                group => group.Key, 
                group => group.Select(editionsData => {
                    return new EditionResultDTO {
                        BookId = bookId,
                        Isbn = editionsData.Value[BnfPropertiesConsts.ISBN],
                        Subtitle = editionsData.Value[BnfPropertiesConsts.SUBTITLE],
                        PublicationDateBnf = editionsData.Value[BnfPropertiesConsts.PUBLICATION_DATE_BNF],
                        Volume = editionsData.Value[BnfPropertiesConsts.VOLUME],
                        Summary = editionsData.Value[BnfPropertiesConsts.SUMMARY],
                        Series = new SeriesResultDTO {
                            SeriesName = editionsData.Value[BnfPropertiesConsts.SERIES_NAME]
                        },
                        Publisher = new PublisherResultDTO {
                            PublishingHouse = editionsData.Value[BnfPropertiesConsts.PUBLISHER]
                        }
                    };
                }).ToList()
            );

            outList.Add(searchResultDto);
            bookId++;
        }

        return outList;
    }

    /// <summary>
    /// Searches for a value into the datafields according to the property name
    /// </summary>
    /// <param name="datafields">List of filtered BnfDataField objects</param>
    /// <param name="propertyName">Name of the property, refers to the file BnfConsts</param>
    /// <returns>A string value</returns>
    private string SearchForValue(ref IEnumerable<BnfDataField> datafields, string propertyName) {

        BnfDataField? extractedDataf;
        string? outValue = string.Empty, 
                extractedValue = string.Empty;

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
                outValue += " " + extractedValue;
            }
        }

        return outValue.Trim();
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