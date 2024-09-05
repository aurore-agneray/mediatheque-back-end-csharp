using ApplicationCore.Interfaces.DTOs;
using System.Xml.Linq;

namespace ApplicationCore.Interfaces;

/// <summary>
/// Defines the structure of XML Repositories classes
/// </summary>
public interface IXMLRepository
{
    /// <summary>
    /// Extracts and returns XML nodes from the concerned data source
    /// </summary>
    /// <returns>An enumerable of XElement objects in an async context</returns>
    public Task<IEnumerable<XElement>> GetXMLResultsNodes<T>(T searchCriteria) where T : class, ISearchDTO;
}