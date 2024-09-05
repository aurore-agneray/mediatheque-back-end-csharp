using ApplicationCore.DTOs.SearchDTOs;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Infrastructure.BnfApi.Repositories;

/// <summary>
/// Retrieves data from the BnF databases with the simple search
/// </summary>
public class BnfApiSimpleSearchRepository : BnfApiSearchRepository
{
    /// <summary>
    /// Constructs the complete string with the conditions to
    /// send to the BnF API
    /// </summary>
    /// <param name="criterion">Criterion entered by the user to launch the searching process</param>
    /// <param name="noticesQty">The quantity of notices returned by the API</param>
    /// <returns>Returns the conditions part of the URL for the request</returns>
    protected string GetSimpleSearchConditions(string criterion, int noticesQty)
    {
        var stringBuilder = new StringBuilder();

        if (string.IsNullOrEmpty(criterion))
        {
            return string.Empty;
        }

        if (noticesQty < BnfGlobalConsts.DEFAULT_NOTICES_NUMBER)
        {
            noticesQty = BnfGlobalConsts.DEFAULT_NOTICES_NUMBER;
        }

        stringBuilder = stringBuilder.Clear();
        stringBuilder.Append(DOUBLE_QUOTES)
                     .Append(criterion.Replace(SPACE, PLUS))
                     .Append(DOUBLE_QUOTES);
        criterion = stringBuilder.ToString();

        return BnfGlobalConsts.SIMPLE_SEARCH_PARAMETERED_CONDITIONS(criterion, noticesQty);
    }

    /// <summary>
    /// Extracts and returns XML nodes from the BnF API
    /// </summary>
    /// <returns>An enumerable of XElement objects</returns>
    public override async Task<IEnumerable<XElement>> GetXMLResultsNodes<ISimpleSearchDTO>(ISimpleSearchDTO searchCriteria)
    {
        SimpleSearchDTO? criteriaDto = null;

        if (searchCriteria is ISimpleSearchDTO idto)
        {
            criteriaDto = searchCriteria as SimpleSearchDTO;
        }
        else
        {
            return default;
        }

        if (string.IsNullOrEmpty(criteriaDto?.SimpleCriterion))
        {
            return default;
        }

        string stringCriterion = criteriaDto.SimpleCriterion;
        var noticesNb = criteriaDto.BnfNoticesQuantity == default ? 20 : criteriaDto.BnfNoticesQuantity;

        var url = BnfGlobalConsts.BNF_API_URL_BASE + GetSimpleSearchConditions(stringCriterion, noticesNb);

        // Configures the XML reader
        var readerSettings = new XmlReaderSettings()
        {
            Async = true
        };

        XDocument fileRoot;

        using (var reader = XmlReader.Create(url, readerSettings))
        {

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
            return default;
        }

        // Checks the results nodes
        return fileRoot.Descendants(_nMxc + RECORD);
    }
}