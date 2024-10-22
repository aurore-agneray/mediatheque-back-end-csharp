using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.DTOs;
using System.Xml.Linq;

namespace Infrastructure.BnfApi.Repositories;

/// <summary>
/// Retrieves data from the BnF databases
/// </summary>
public abstract class BnfApiSearchRepository : IXMLRepository
{
    /// <summary>
    /// Extracts and returns XML nodes from the BnF API
    /// </summary>
    /// <returns>An enumerable of XElement objects in an async context</returns>
    public abstract Task<IEnumerable<XElement>> GetXMLResultsNodes<T>(T searchCriteria) where T : class, ISearchDTO;
}