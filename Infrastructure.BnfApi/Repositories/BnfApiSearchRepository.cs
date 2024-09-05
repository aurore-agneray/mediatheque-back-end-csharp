using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.DTOs;
using System.Xml.Linq;

namespace Infrastructure.BnfApi.Repositories;

/// <summary>
/// Retrieves data from the BnF databases
/// </summary>
public abstract class BnfApiSearchRepository : IXMLRepository
{
    protected const string DOUBLE_QUOTES = "\"";
    protected const string SPACE = " ";
    protected const string PLUS = "+";
    protected const string NUMBER_OF_RECORDS = "numberOfRecords";
    protected const string RECORD = "record";

    /// <summary>
    /// Namespace "mxc"
    /// </summary>
    protected readonly XNamespace _nMxc = "info:lc/xmlns/marcxchange-v2";

    /// <summary>
    /// Namespace "srw"
    /// </summary>
    protected readonly XNamespace _nSrw = "http://www.loc.gov/zing/srw/";

    /// <summary>
    /// Extracts and returns XML nodes from the BnF API
    /// </summary>
    /// <returns>An enumerable of XElement objects in an async context</returns>
    public abstract Task<IEnumerable<XElement>> GetXMLResultsNodes<T>(T searchCriteria) where T : class, ISearchDTO;
}