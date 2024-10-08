using ApplicationCore.Interfaces;

namespace MediathequeBackCSharp.Services.Aggregates;

/// <summary>
/// An object with all simple search services, injected by DI.
/// Allows the selection of a specific service into the managers.
/// </summary>
/// <remarks>
/// Main constructor of the class AllSearchServices.
/// Gets its properties by dependency injection
/// </remarks>
/// <param name="sqlSimpleSearch">Simple search service for the MySQL database</param>
/// <param name="bnfApiSimpleSearch">Simple search service the BnF API</param>
public class AllSimpleSearchServices(MySQLSimpleSearchService sqlSimpleSearch, BnfApiSimpleSearchService bnfApiSimpleSearch) : IAllSearchServices
{
    /// <summary>
    /// Simple search service for the MySQL database
    /// </summary>
    public MySQLSimpleSearchService MySQLSimpleSearchService { get; set; } = sqlSimpleSearch;

    /// <summary>
    /// Simple search service for the BnF API
    /// </summary>
    public BnfApiSimpleSearchService BnfApiSimpleSearchService { get; set; } = bnfApiSimpleSearch;
}