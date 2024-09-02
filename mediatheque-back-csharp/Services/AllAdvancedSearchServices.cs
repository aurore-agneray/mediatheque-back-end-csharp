using ApplicationCore.Interfaces;

namespace mediatheque_back_csharp.Services;

/// <summary>
/// An object with all search services, injected by DI.
/// Allows the selection of a specific service into the managers.
/// </summary>
public class AllAdvancedSearchServices : IAllSearchServices
{
    /// <summary>
    /// Advanced search service for the MySQL database
    /// </summary>
    public MySQLAdvancedSearchService MySQLAdvancedSearchService { get; set; }

    /// <summary>
    /// Main constructor of the class AllSearchServices.
    /// Gets its properties by dependency injection
    /// </summary>
    /// <param name="sqlAdvancedSearch">Advanced search service for the MySQL database</param>
    public AllAdvancedSearchServices(MySQLAdvancedSearchService sqlAdvancedSearch)
    {
        MySQLAdvancedSearchService = sqlAdvancedSearch;
    }
}