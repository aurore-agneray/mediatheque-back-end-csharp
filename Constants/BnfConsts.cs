using mediatheque_back_csharp.Classes;

namespace mediatheque_back_csharp.Constants;

/// <summary>
/// Constants used into the Bnf Search
/// </summary>
public static class BnfConsts {

    /// <summary>
    /// Base of the url for the Bnf's requests
    /// </summary>
    public const string BNF_API_URL_BASE = "http://catalogue.bnf.fr/api/SRU?version=1.2&operation=searchRetrieve&query=";

    /// <summary>
    /// Joins tags and codes used to extract books data from
    /// the XML BnF API result
    /// </summary>
    public static Dictionary<string, Dictionary<BnfDataField, string[]>> TAGS_AND_CODES => new() {
        {
            BnfPropertiesConsts.AUTHOR,
            new() {
                { BnfDefaultDatafieldsConsts._DATA_FIELD_200, ["f"] },
                { BnfDefaultDatafieldsConsts._DATA_FIELD_700, ["a", "b"] },
                { BnfDefaultDatafieldsConsts._DATA_FIELD_710, ["a"] }
            }
        },
        {
            BnfPropertiesConsts.ISBN,
            new() {
                { BnfDefaultDatafieldsConsts._DATA_FIELD_010, ["a"] }
            }
        },
        {
            BnfPropertiesConsts.PUBLICATION_DATE_BNF,
            new() {
                { BnfDefaultDatafieldsConsts._DATA_FIELD_210, ["d"] },
                { BnfDefaultDatafieldsConsts._DATA_FIELD_214, ["d"] }
            }
        },
        {
            BnfPropertiesConsts.PUBLISHER,
            new() {
                { BnfDefaultDatafieldsConsts._DATA_FIELD_210, ["c"] },
                { BnfDefaultDatafieldsConsts._DATA_FIELD_214, ["c"] }
            }
        },
        {
            BnfPropertiesConsts.SERIES_NAME,
            new() {
                { BnfDefaultDatafieldsConsts._DATA_FIELD_225_1_9, ["a"] },
                { BnfDefaultDatafieldsConsts._DATA_FIELD_461, ["t"] }
            }
        },
        {
            BnfPropertiesConsts.SUBTITLE,
            new() {
                { BnfDefaultDatafieldsConsts._DATA_FIELD_200, ["i"] }
            }
        },
        {
            BnfPropertiesConsts.SUMMARY,
            new() {
                { BnfDefaultDatafieldsConsts._DATA_FIELD_330, ["a"] }
            }
        },
        {
            BnfPropertiesConsts.TITLE,
            new() {
                { BnfDefaultDatafieldsConsts._DATA_FIELD_200, ["a", "e"] }
            }
        },
        {
            BnfPropertiesConsts.VOLUME,
            new() {
                { BnfDefaultDatafieldsConsts._DATA_FIELD_225_1_9, ["v"] },
                { BnfDefaultDatafieldsConsts._DATA_FIELD_461, ["v"] }
            }
        }
    };

    /// <summary>
    /// Properties that are retrieved for each edition
    /// </summary>
    public static List<string> EXTRACTED_PROPERTIES => TAGS_AND_CODES.Keys.ToList();

    /// <summary>
    /// Tags that are searched into the XML content
    /// </summary>
    public static List<string> NEEDED_TAGS => TAGS_AND_CODES.SelectMany(
        item => item.Value.Select(tagAndCodes => tagAndCodes.Key.Tag)
    ).Distinct().ToList();

    /// <summary>
    /// Returns the parametered part of the simple search request's url
    /// </summary>
    public static Func<string, int, string> SIMPLE_SEARCH_PARAMETERED_CONDITIONS = (string criterion, int noticesQty) => {

        return string.Concat(
            $"bib.author all {criterion} or bib.title all {criterion} ",
            $"or bib.isbn all {criterion} or bib.serialtitle all {criterion} ",
            $"&recordSchema=unimarcxchange&maximumRecords={noticesQty}&startRecord=1"
        );
    };
}