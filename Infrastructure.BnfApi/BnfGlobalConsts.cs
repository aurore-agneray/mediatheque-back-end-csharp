using System.Xml.Linq;

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
    /// The numbers allowed by the BnF API for defining the quantity of retrieved notices.
    /// Check here : http://catalogue.bnf.fr/api/test.do
    /// </summary>
    public static readonly int[] ALLOWED_NOTICES_NUMBERS = [20, 100, 200, 500, 1000];

    /// <summary>
    /// Defines the default value for the properties Ind1 and Ind2 of the Datafields
    /// </summary>
    public const string IND_DEFAULT_VALUE = " ";

    /// <summary>
    /// Used for separating the title and the author's name
    /// when they are concatenated
    /// </summary>
    public const string TITLE_AND_AUTHOR_NAME_SEPARATOR = ";;;";

    /// <summary>
    /// Namespace "mxc"
    /// </summary>
    public static readonly XNamespace NAMESPACE_MXC = "info:lc/xmlns/marcxchange-v2";

    /// <summary>
    /// Namespace "srw"
    /// </summary>
    public static readonly XNamespace NAMESPACE_SRW = "http://www.loc.gov/zing/srw/";

    /// <summary>
    /// Returns the parametered part of the simple search request's url
    /// </summary>
    public static readonly Func<string, int, string> SIMPLE_SEARCH_PARAMETERED_CONDITIONS = (string criterion, int noticesQty) => {

        /* DON'T TRY to write this request as a single unit with multilines ... 
        ** the spaces at the end of each line won't be take in account */
        return string.Concat(
            $"bib.author all {criterion} or bib.title all {criterion} ",
            $"or bib.isbn all {criterion} ",
            $"&recordSchema=unimarcxchange&maximumRecords={noticesQty}&startRecord=1"
        );
    };
}