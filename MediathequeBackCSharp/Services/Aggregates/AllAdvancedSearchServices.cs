using ApplicationCore.Interfaces;

namespace MediathequeBackCSharp.Services.Aggregates;

/// <summary>
/// An object with all search services, injected by DI.
/// Allows the selection of a specific service into the managers.
/// </summary>
/// <remarks>
/// Main constructor of the class AllSearchServices.
/// Gets its properties by dependency injection
/// </remarks>
/// <param name="sqlAdvancedSearch">Advanced search service for the MySQL database</param>
public class AllAdvancedSearchServices(MySQLAdvancedSearchService sqlAdvancedSearch) : IAllSearchServices
{
    /// <summary>
    /// Advanced search service for the MySQL database
    /// </summary>
    public MySQLAdvancedSearchService MySQLAdvancedSearchService { get; set; } = sqlAdvancedSearch;
}