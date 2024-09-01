using ApplicationCore.DTOs.SearchDTOs.CriteriaDTOs;
using ApplicationCore.Pocos;

namespace ApplicationCore.Interfaces.Databases;

/// <summary>
/// Defines the structure of SQLRequests classes
/// </summary>
public interface ISQLRequests<T, U> 
    where T : class, IDatabaseSettings
    where U : IMediathequeDbContext<T>
{
    /// <summary>
    /// Context for querying a SQL database
    /// </summary>
    public U DbContext { get; }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="searchCriteria">Contains the criterion sent by the client</param>
    /// <returns>A IQueryable<Book> object</returns>
    public IQueryable<Book> GetOrderedBooksRequest(SearchCriteriaDTO searchCriteria);

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the editions from the database
    /// </summary>
    /// <param name="bookIds">List of the IDs of the concerned books</param>
    /// <returns>A IQueryable<Edition> object</returns>
    public IQueryable<Edition> GetEditionsForSeveralBooksRequest(int[] bookIds);
}
