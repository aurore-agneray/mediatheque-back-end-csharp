using System.Text.RegularExpressions;

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
    public static Dictionary<string, Dictionary<string, string[]>> TAGS_AND_CODES => new() {
        {
            "author",
            new() {
                { "200", ["f"] },
                { "700", ["a", "b"] },
                { "710", ["a"] }
            }
        },
        {
            "isbn",
            new() {
                { "010", ["32", "656"] }
            }
        },
        {
            "publicationDateBnf",
            new() {
                { "210", ["d"] },
                { "214", ["d"] }
            }
        },
        {
            "publisher",
            new() {
                { "210", ["c"] },
                { "214", ["c"] }
            }
        },
        {
            "seriesName",
            new() {
                { "225|1|9", ["a"] },
                { "461", ["t"] }
            }
        },
        {
            "subtitle",
            new() {
                { "200", ["i"] }
            }
        },
        {
            "summary",
            new() {
                { "330", ["a"] }
            }
        },
        {
            "title",
            new() {
                { "200", ["a", "e"] }
            }
        },
        {
            "volume",
            new() {
                { "225|1|9", ["v"] },
                { "461", ["v"] }
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
        item => item.Value.Select(tagAndCodes => Regex.Match(tagAndCodes.Key, @"^\d{3}").Value)
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