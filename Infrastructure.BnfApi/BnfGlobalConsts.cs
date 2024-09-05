namespace Infrastructure.BnfApi;

/// <summary>
/// Contains constants that can be used everywhere for the BnF instructions
/// </summary>
public class BnfGlobalConsts
{
    /// <summary>
    /// Base of the url for the Bnf's requests
    /// </summary>
    public const string BNF_API_URL_BASE = "http://catalogue.bnf.fr/api/SRU?version=1.2&operation=searchRetrieve&query=";

    /// <summary>
    /// The default max number of notices returned by the API
    /// </summary>
    public const int DEFAULT_NOTICES_NUMBER = 20;

    /// <summary>
    /// Returns the parametered part of the simple search request's url
    /// </summary>
    public static readonly Func<string, int, string> SIMPLE_SEARCH_PARAMETERED_CONDITIONS = (string criterion, int noticesQty) => {

        return string.Concat(
            $"bib.author all {criterion} or bib.title all {criterion} ",
            $"or bib.isbn all {criterion} ",
            $"&recordSchema=unimarcxchange&maximumRecords={noticesQty}&startRecord=1"
        );
    };
}