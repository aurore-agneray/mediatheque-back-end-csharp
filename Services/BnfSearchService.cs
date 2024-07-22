using mediatheque_back_csharp.Classes;
using mediatheque_back_csharp.Constants;
using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Extensions;
using mediatheque_back_csharp.Generators;
using mediatheque_back_csharp.Interfaces;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace mediatheque_back_csharp.Services;

/// <summary>
/// Methods for preparing the data retrieved from the Bnf's API
/// </summary>
public class BnfSearchService : ISearchService
{
    private const string DOUBLE_QUOTES = "\"";
    private const string SPACE = " ";
    private const string PLUS = "+";
    private const string DATAFIELD = "datafield";
    private const string TAG = "tag";
    private const string IND1 = "ind1";
    private const string IND2 = "ind2";
    private const string CODE = "code";
    private const string NUMBER_OF_RECORDS = "numberOfRecords";
    private const string RECORD = "record";

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
    private readonly XNamespace _nMxc = "info:lc/xmlns/marcxchange-v2";

    /// <summary>
    /// Namespace "srw"
    /// </summary>
    private readonly XNamespace _nSrw = "http://www.loc.gov/zing/srw/";

    /// <summary>
    /// StringBuilder for concatenating strings
    /// </summary>
    private StringBuilder _stringBuilder = new StringBuilder();

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

        if (noticesQty < BnfConsts.DEFAULT_NOTICES_NUMBER) {
            noticesQty = BnfConsts.DEFAULT_NOTICES_NUMBER;
        }

        _stringBuilder = _stringBuilder.Clear();
        _stringBuilder.Append(DOUBLE_QUOTES)
                     .Append(criterion.Replace(SPACE, PLUS))
                     .Append(DOUBLE_QUOTES);
        criterion = _stringBuilder.ToString();

        return BnfConsts.SIMPLE_SEARCH_PARAMETERED_CONDITIONS(criterion, noticesQty);
    }

    /// <summary>
    /// Extracts the DataField nodes' data from the given XElement
    /// which corresponds to an edition
    /// </summary>
    /// <param name="result">Object of type XElement</param>
    /// <returns>A list of BnfDataFields</returns>
    private IEnumerable<BnfDataField> GetDataFieldsFromXElement(XElement result) {

        return result.Descendants(_nMxc + DATAFIELD)
            .Where(node => 
                node.Attributes().ToList().Exists(
                    at => at.Name == TAG
                    && BnfConsts.NEEDED_TAGS.Contains(at.Value)
                )
            )
            .Select(rawDataField => new BnfDataField() {
                Tag = rawDataField.Attribute(TAG).Value,
                Ind1 = rawDataField.Attribute(IND1).Value,
                Ind2 = rawDataField.Attribute(IND2).Value,
                Subfields = rawDataField.Descendants().Select(rawSubfield => 
                    new BnfSubField {
                        Code = rawSubfield.Attribute(CODE).Value,
                        Value = rawSubfield.Value
                    }
                )
            });
    }

    /// <summary>
    /// Searches for a value into the datafields according to the property name
    /// </summary>
    /// <param name="datafields">List of filtered BnfDataField objects</param>
    /// <param name="propertyName">Name of the property, refers to the file BnfConsts</param>
    /// <returns>A string value</returns>
    private string SearchForValue(IEnumerable<BnfDataField> datafields, string propertyName) {

        BnfDataField? extractedDataf;
        string? extractedValue = string.Empty;

        // Gets the convenient tags and codes for the given property name
        var searchedTagsAndCodes = BnfConsts.TAGS_AND_CODES[propertyName];

        /* "searchParams" contains a tag, ind1, ind2 and the bound codes 
        ** The different tags are explored while no value is found */
        foreach (var searchParams in searchedTagsAndCodes) {

            // Takes the BnfDataField corresponding to the tag
            extractedDataf = datafields.FirstOrDefault(
                dataf => dataf.Equals(searchParams.Key)
            );
            
            if (extractedDataf == null) {
                continue;
            }

            /* Searches for the value into the subfields whose codes are given
            ** If there are several explored subfields into the same datafield, the values are aggregated */
            extractedValue = searchParams.Value.Select(code =>
                extractedDataf.ExtractValueFromSubfield(code)
            ).Aggregate((agg, next) =>
                agg + " " + (next ?? string.Empty)
            );

            if (!string.IsNullOrEmpty(extractedValue)) {
                break;
            }
        }

        return !string.IsNullOrEmpty(extractedValue) 
                ? extractedValue.Trim() 
                : string.Empty;
    }

    /// <summary>
    /// Extracts the properties of the edition from the given BnfDataField list.
    /// The names of these properties are defined into the class BnfPropertiesConsts 
    /// </summary>
    /// <param name="datafields">List of BnfDataField objects</param>
    /// <returns>An KeyValuePair whose key is into the format "AUTHORNAME_BOOKNAME" 
    /// and the value is a dictionary containing the edition's data</returns>
    private KeyValuePair<string, Dictionary<string, string>> ExtractOneEditionData(IEnumerable<BnfDataField> datafields) {

        string isbn;

        // Keep the result only if the ISBN is not empty
        if (!string.IsNullOrEmpty(isbn = SearchForValue(datafields, BnfPropertiesConsts.ISBN))) {

            _stringBuilder = _stringBuilder.Clear();
            _stringBuilder.Append(SearchForValue(datafields, BnfPropertiesConsts.TITLE))
                         .Append(BnfConsts.TITLE_AND_AUTHOR_NAME_SEPARATOR)
                         .Append(SearchForValue(datafields, BnfPropertiesConsts.AUTHOR));

            return new(
                _stringBuilder.ToString(),
                new() {
                    { BnfPropertiesConsts.ISBN, isbn },
                    { BnfPropertiesConsts.PUBLICATION_DATE_BNF, SearchForValue(datafields, BnfPropertiesConsts.PUBLICATION_DATE_BNF) },
                    { BnfPropertiesConsts.PUBLISHER, SearchForValue(datafields, BnfPropertiesConsts.PUBLISHER) },
                    { BnfPropertiesConsts.SERIES_NAME, SearchForValue(datafields, BnfPropertiesConsts.SERIES_NAME) },
                    { BnfPropertiesConsts.SUBTITLE, SearchForValue(datafields, BnfPropertiesConsts.SUBTITLE) },
                    { BnfPropertiesConsts.SUMMARY, SearchForValue(datafields, BnfPropertiesConsts.SUMMARY) },
                    { BnfPropertiesConsts.VOLUME, SearchForValue(datafields, BnfPropertiesConsts.VOLUME) }
                }
            );
        }

        return default;
    }

    /// <summary>
    /// Extracts data from XElements of the XML content in order 
    /// to store them into dictionaries
    /// </summary>
    /// <param name="xmlNodes">XML nodes</param>
    /// <returns>Returns objects whose keys are into the format "AUTHORNAME_BOOKNAME" 
    /// and the values are dictionaries containing editions data</returns>
    private List<KeyValuePair<string, Dictionary<string, string>>> ExtractResultsFromXmlNodes(IEnumerable<XElement> xmlNodes) {
        
        IEnumerable<BnfDataField> datafields;
        KeyValuePair<string, Dictionary<string, string>> extractedEditionData;
        List<KeyValuePair<string, Dictionary<string, string>>> results = new();

        foreach (var result in xmlNodes) {
            datafields = GetDataFieldsFromXElement(result);
            extractedEditionData = ExtractOneEditionData(datafields);

            if (!extractedEditionData.Equals(default(KeyValuePair<string, Dictionary<string, string>>))) {
                results.Add(extractedEditionData);
            }
        }

        return results.ToList();
    }

    /// <summary>
    /// Converts the given data into DTOs and order them by ascending book's title and author's name
    /// </summary>
    /// <param name="extractedResults">Results that have been extracted from the XML nodes
    /// and stored into dictionaries</param>
    /// <returns>A list of SearchResultDTOs</returns>
    private List<SearchResultDTO> ConvertExtractedResultsIntoDtos(List<KeyValuePair<string, Dictionary<string, string>>> extractedResults) {

        var outList = new List<SearchResultDTO>();
        SearchResultDTO searchResultDto;
        List<EditionResultDTO> editionsDtos;

        // Groups the results by book and author's name
        var groupedByBooks = extractedResults.GroupBy(res => res.Key);

        // Creates the DTOs
        int bookId = 1;
        int editionId = 1;

        foreach (var book in groupedByBooks) {

            // Creates for each book a SearchResultDTO that will contain several editions
            searchResultDto = new() {
                BookId = bookId,
                Book = ResultsDtosGenerator.GenerateBookResultDTO(book.Key, bookId)
            };

            // Generates the editions' DTOs
            editionsDtos = book.Select(edition => 
                ResultsDtosGenerator.GenerateEditionResultDTO(edition.Value, bookId)
            ).OrderElementsByVolume().ToList();

            editionsDtos.ForEach(ed => ed.Id = editionId++);

            // Groups the concerned editions by the series' name and completes the list for the concerned book
            searchResultDto.Editions = editionsDtos.GroupElementsBySeriesName();

            outList.Add(searchResultDto);
            bookId++;
            editionId = 1;
        }

        return outList.Where(dto => dto?.Book?.Author != null)
                      .OrderBy(dto => dto.Book.Title)
                      .ThenBy(dto => dto.Book.Author.CompleteName)
                      .ToList();
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
    /// <param name="noticesNb">Max number of notices returned by the BnF</param>
    /// <returns>A list of SearchResultDTOs</returns>
    public async Task<IEnumerable<SearchResultDTO>> GetResults(object criterion, int noticesNb) {

        // For a simple search, the criterion should be a string value !
        if (criterion.GetType() != typeof(string)) {
            throw new ArgumentException(TextConsts.BNF_SEARCH_SERVICE_ERROR_CRITERION_TYPE);
        }

        var outResults = new List<SearchResultDTO>();
        var stringCriterion = (string)criterion;
        var url = BnfConsts.BNF_API_URL_BASE + GetSimpleSearchConditions(stringCriterion, noticesNb);

        // Configures the XML reader
        var readerSettings = new XmlReaderSettings() {
            Async = true
        };

        XDocument fileRoot;

        using (var reader = XmlReader.Create(url, readerSettings)) {

            // Loads the XML content from the url
            fileRoot = await XDocument.LoadAsync(
                reader, 
                LoadOptions.None, 
                CancellationToken.None
            );
        }

        // Checks the number of records
        var recordsNumberNode = fileRoot.Descendants(_nSrw + NUMBER_OF_RECORDS)
                                        .FirstOrDefault();

        if (!int.TryParse(recordsNumberNode?.Value, out int resultsNumber) || resultsNumber <= 0)
        {
            return new List<SearchResultDTO>();
        }

        // Checks the results nodes
        var resultsNodes = fileRoot.Descendants(_nMxc + RECORD);

        if (resultsNodes == null || resultsNodes.Count() <= 0)
        {
            return new List<SearchResultDTO>();
        }

        var extractedResults = ExtractResultsFromXmlNodes(resultsNodes);
        outResults = ConvertExtractedResultsIntoDtos(extractedResults);

        return outResults;
    }
}