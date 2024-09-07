using ApplicationCore.Interfaces;

namespace MediathequeBackCSharp.Services.Aggregates;

/// <summary>
/// An object with all simple search services, injected by DI.
/// Allows the selection of a specific service into the managers.
/// </summary>
public class AllSimpleSearchServices : IAllSearchServices
{
    /// <summary>
    /// Simple search service for the MySQL database
    /// </summary>
    public MySQLSimpleSearchService MySQLSimpleSearchService { get; set; }

    /// <summary>
    /// Simple search service for the BnF API
    /// </summary>
    public BnfApiSimpleSearchService BnfApiSimpleSearchService { get; set; }

    /// <summary>
    /// Main constructor of the class AllSearchServices.
    /// Gets its properties by dependency injection
    /// </summary>
    /// <param name="sqlSimpleSearch">Simple search service for the MySQL database</param>
    /// <param name="bnfApiSimpleSearch">Simple search service the BnF API</param>
    public AllSimpleSearchServices(MySQLSimpleSearchService sqlSimpleSearch, BnfApiSimpleSearchService bnfApiSimpleSearch)
    {
        MySQLSimpleSearchService = sqlSimpleSearch;
        BnfApiSimpleSearchService = bnfApiSimpleSearch;
    }
}