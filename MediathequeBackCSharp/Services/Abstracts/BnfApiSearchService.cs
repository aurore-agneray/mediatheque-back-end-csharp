using ApplicationCore.AbstractServices;
using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.BnfApi;
using Infrastructure.BnfApi.Constants;
using Infrastructure.BnfApi.POCOs;
using LinqKit;
using MediathequeBackCSharp.Generators;
using MediathequeBackCSharp.Texts;
using System.Resources;
using System.Text;
using System.Xml.Linq;

namespace MediathequeBackCSharp.Services.Abstracts;

/// <summary>
/// Methods for preparing the data extracted from the XML BnF's content with the simple search
/// </summary>
public abstract class BnfApiSearchService : SearchService
{
    private const string DATAFIELD = "datafield";
    private const string TAG = "tag";
    private const string IND1 = "ind1";
    private const string IND2 = "ind2";
    private const string CODE = "code";

    /// <summary>
    /// Repository used for retrieving data from a given XML content
    /// </summary>
    protected readonly IXMLRepository _xmlRepository;

    /// <summary>
    /// Namespace "mxc"
    /// </summary>
    protected readonly XNamespace _nMxc = "info:lc/xmlns/marcxchange-v2";

    /// <summary>
    /// Namespace "srw"
    /// </summary>
    protected readonly XNamespace _nSrw = "http://www.loc.gov/zing/srw/";

    /// <summary>
    /// Constructor of the BnfApiSearchService class
    /// </summary>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    /// <param name="xmlRepo">Repository for collecting data from XML content</param>
    protected BnfApiSearchService(IMapper mapper, ResourceManager textsManager, IXMLRepository xmlRepo)
        : base(mapper, textsManager)
    {
        _xmlRepository = xmlRepo;
    }

    /// <summary>
    /// Extracts the DataField nodes' data from the given XElement
    /// which corresponds to an edition
    /// </summary>
    /// <param name="result">Object of type XElement</param>
    /// <returns>A list of BnfDataFields</returns>
    private IEnumerable<BnfDataField> GetDataFieldsFromXElement(XElement result)
    {
        return result.Descendants(_nMxc + DATAFIELD)
            .Where(node =>
                node.Attributes().ToList().Exists(
                    at => at.Name == TAG
                    && BnfTagsAndCodesConsts.NEEDED_TAGS.Contains(at.Value)
                )
            )
            .Select(rawDataField => new BnfDataField()
            {
                Tag = rawDataField.Attribute(TAG).Value,
                Ind1 = rawDataField.Attribute(IND1).Value,
                Ind2 = rawDataField.Attribute(IND2).Value,
                Subfields = rawDataField.Descendants().Select(rawSubfield =>
                    new BnfSubField
                    {
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
    private string SearchForValue(IEnumerable<BnfDataField> datafields, string propertyName)
    {
        BnfDataField? extractedDataf;
        string? extractedValue = string.Empty;

        // Gets the convenient tags and codes for the given property name
        var searchedTagsAndCodes = BnfTagsAndCodesConsts.TAGS_AND_CODES[propertyName];

        /* "searchParams" contains a tag, ind1, ind2 and the bound codes 
        ** The different tags are explored while no value is found */
        foreach (var searchParams in searchedTagsAndCodes)
        {
            // Takes the BnfDataField corresponding to the tag
            extractedDataf = datafields.FirstOrDefault(
                dataf => dataf.Equals(searchParams.Key)
            );

            if (extractedDataf == null)
            {
                continue;
            }

            /* Searches for the value into the subfields whose codes are given
            ** If there are several explored subfields into the same datafield, the values are aggregated */
            extractedValue = searchParams.Value.Select(code =>
                extractedDataf.ExtractValueFromSubfield(code)
            ).Aggregate((agg, next) =>
                agg + " " + (next ?? string.Empty)
            );

            if (!string.IsNullOrEmpty(extractedValue))
            {
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
    private KeyValuePair<string, Dictionary<string, string>> ExtractOneEditionData(IEnumerable<BnfDataField> datafields)
    {
        string isbn;

        // Keep the result only if the ISBN is not empty
        if (!string.IsNullOrEmpty(isbn = SearchForValue(datafields, BnfPropertiesConsts.ISBN)))
        {
            var stringBuilder = new StringBuilder();

            stringBuilder = stringBuilder.Clear();
            stringBuilder.Append(SearchForValue(datafields, BnfPropertiesConsts.TITLE))
                         .Append(BnfGlobalConsts.TITLE_AND_AUTHOR_NAME_SEPARATOR)
                         .Append(SearchForValue(datafields, BnfPropertiesConsts.AUTHOR));
            return new(
                stringBuilder.ToString(),
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
    private List<KeyValuePair<string, Dictionary<string, string>>> ExtractEditionsFromXmlNodes(IEnumerable<XElement> xmlNodes)
    {
        IEnumerable<BnfDataField> datafields;
        KeyValuePair<string, Dictionary<string, string>> extractedEditionData;
        List<KeyValuePair<string, Dictionary<string, string>>> results = new();

        foreach (var result in xmlNodes)
        {
            datafields = GetDataFieldsFromXElement(result);
            extractedEditionData = ExtractOneEditionData(datafields);

            if (!extractedEditionData.Equals(default(KeyValuePair<string, Dictionary<string, string>>)))
            {
                results.Add(extractedEditionData);
            }
        }

        return results.ToList();
    }

    /// <summary>
    /// Extracts the books and their editions from the concerned XML repository
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    protected override async Task<Tuple<List<BookResultDTO>, List<EditionResultDTO>>> ExtractDataFromRepository(SearchDTO searchCriteria)
    {
        List<BookResultDTO> booksList = new List<BookResultDTO>();
        List<EditionResultDTO> editionsList = new List<EditionResultDTO>();

        // Checks the given notices' quantity before continuing
        if (!BnfGlobalConsts.ALLOWED_NOTICES_NUMBERS.Contains(searchCriteria.BnfNoticesQuantity))
        {
            throw new Exception(
                TextsManager.GetString(TextsKeys.ERROR_BNF_NOTICES_NUMBER) ?? string.Empty
            );
        }

        // Checks the availability of the repository
        if (_xmlRepository == null)
        {
            throw new ArgumentNullException(nameof(_xmlRepository));
        }

        // Extracts and formats data
        IEnumerable<XElement> xmlExtractedData = await _xmlRepository.GetXMLResultsNodes(searchCriteria);
        List<KeyValuePair<string, Dictionary<string, string>>> editionsDefinedByBookAndAuthor = ExtractEditionsFromXmlNodes(xmlExtractedData);

        // Groups the results by book and author's name
        var editionsGroupedByBook = editionsDefinedByBookAndAuthor.GroupBy(res => res.Key);

        // Creates the DTOs with fictional IDs
        int bookId = 1;
        int editionId = 1;

        foreach (var bookGroup in editionsGroupedByBook)
        {
            // Add the book in the final list
            booksList.Add(
                BnfResultsDtosGenerator.GenerateBookResultDTO(bookGroup.Key, bookId)
            );

            // Generates the editions' DTOs
            var editionsDtos = bookGroup.Select(edition =>
                BnfResultsDtosGenerator.GenerateEditionResultDTO(edition.Value, bookId)
            );

            editionsDtos.ForEach(ed => ed.Id = editionId++);

            // Add its editions into the final list
            editionsList.AddRange(editionsDtos);

            bookId++;
            editionId = 1;
        }

        return new (booksList, editionsList);
    }
}