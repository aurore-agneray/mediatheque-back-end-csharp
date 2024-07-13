/// <summary>
/// Constants used into the Bnf Search
/// </summary>
public static class BnfConsts {

    /// <summary>
    /// Base of the url for the Bnf's requests
    /// </summary>
    public const string BNF_API_URL_BASE = "http://catalogue.bnf.fr/api/SRU?version=1.2&operation=searchRetrieve&query=";

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