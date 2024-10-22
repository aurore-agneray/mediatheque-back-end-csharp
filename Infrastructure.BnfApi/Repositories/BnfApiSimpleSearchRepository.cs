using ApplicationCore.DTOs.SearchDTOs;
using System.Xml.Linq;

namespace Infrastructure.BnfApi.Repositories;

/// <summary>
/// Retrieves data from the BnF databases with the simple search
/// </summary>
public class BnfApiSimpleSearchRepository : BnfApiSearchRepository
{
    /// <summary>
    /// Extracts and returns XML nodes from the BnF API
    /// </summary>
    /// <returns>An enumerable of XElement objects objects that are "records"</returns>
    public override async Task<IEnumerable<XElement>> GetXMLResultsNodes<ISimpleSearchDTO>(ISimpleSearchDTO searchCriteria)
    {
        SimpleSearchDTO? criteriaDto;

#pragma warning disable IDE0150
        if (searchCriteria is ISimpleSearchDTO)
        {
            criteriaDto = searchCriteria as SimpleSearchDTO;
        }
        else
        {
            return [];
        }
#pragma warning restore IDE0150

        if (string.IsNullOrEmpty(criteriaDto?.SimpleCriterion))
        {
            return [];
        }

        // Prepares parameters to send to the API
        string stringCriterion = criteriaDto.SimpleCriterion;
        var noticesNb = criteriaDto.BnfNoticesQuantity == default ? 20 : criteriaDto.BnfNoticesQuantity;

        var url = BnfApiStaticManager.GetCompleteUrl(stringCriterion, noticesNb);

        // Retrieves data from the API
        XDocument bnfData = await BnfApiStaticManager.CallBnfApi(url);

        // Search for the relevant nodes into the data
        return BnfApiStaticManager.ExtractXMLResultsNodes(bnfData);
    }
}