using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Interfaces.DTOs;
using ApplicationCore.Pocos;

namespace Infrastructure.MySQL.Repositories;

/// <summary>
/// Contains common methods for MySQL simple and advanced search repositories
/// </summary>
/// <remarks>
/// Main constructor
/// </remarks>
/// <param name="context">Database context</param>
public abstract class MySQLSearchRepository(MySQLDbContext context) : MySQLRepository(context), ISQLSearchRepository<MySQLDbContext>
{
    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="searchCriteria">Criteria sent by the client</param>
    /// <returns>A IQueryable<Book> object</returns>
    public abstract IQueryable<Book>? GetOrderedBooksRequest<T>(T searchCriteria) where T : class, ISearchDTO;

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the editions from the database
    /// </summary>
    /// <param name="bookIds">List of the IDs of the concerned books</param>
    /// <returns>A IQueryable<Edition> object</returns>
    public IQueryable<Edition> GetEditionsForSeveralBooksRequest(int[] bookIds)
    {
        return from edition in DbContext.Editions
               where bookIds.Contains(edition.BookId)
               select edition;
    }
}