namespace mediatheque_back_csharp.Services;

/// <summary>
/// An object with all search services, injected by DI.
/// Allows the selection of a specific service into the managers.
/// </summary>
public class AllSearchServices
{
    /// <summary>
    /// Simple search service for the MySQL database
    /// </summary>
    public MySQLSimpleSearchService MySQLSimpleSearchService { get; set; }

    /// <summary>
    /// Advanced search service for the MySQL database
    /// </summary>
    public MySQLAdvancedSearchService MySQLAdvancedSearchService { get; set; }

    /// <summary>
    /// Main constructor of the class AllSearchServices.
    /// Gets its properties by dependency injection
    /// </summary>
    /// <param name="sqlSimpleSearch">Simple search service for the MySQL database</param>
    /// <param name="sqlAdvancedSearch">Advanced search service for the MySQL database</param>
    public AllSearchServices(MySQLSimpleSearchService sqlSimpleSearch, MySQLAdvancedSearchService sqlAdvancedSearch)
    {
        MySQLSimpleSearchService = sqlSimpleSearch;
        MySQLAdvancedSearchService = sqlAdvancedSearch;
    }
}