using ApplicationCore.Interfaces.DTOs;
using ApplicationCore.Pocos;

namespace ApplicationCore.Interfaces.Databases;

/// <summary>
/// Defines the structure of SQL Repositories classes
/// </summary>
public interface ISQLRepository<out T> where T : IMediathequeDbContextFields
{
    /// <summary>
    /// Context for querying a SQL database
    /// </summary>
    public T DbContext { get; }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="searchCriteria">Contains the criterion sent by the client</param>
    /// <returns>A IQueryable<Book> object</returns>
    public IQueryable<Book> GetOrderedBooksRequest<U>(U searchCriteria) where U : class, ISearchDTO;

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the editions from the database
    /// </summary>
    /// <param name="bookIds">List of the IDs of the concerned books</param>
    /// <returns>A IQueryable<Edition> object</returns>
    public IQueryable<Edition> GetEditionsForSeveralBooksRequest(int[] bookIds);

    /// <summary>
    /// Indicates if the database is available or not
    /// </summary>
    /// <returns>A boolean value</returns>
    public bool IsDatabaseAvailable();
}