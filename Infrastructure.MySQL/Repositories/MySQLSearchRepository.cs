using ApplicationCore.DTOs.SearchDTOs.CriteriaDTOs;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;

namespace Infrastructure.MySQL.Repositories;

/// <summary>
/// Contains common methods for MySQL simple and advanced search repositories
/// </summary>
public abstract class MySQLSearchRepository : ISQLRepository<MySQLDbContext>
{
    /// <summary>
    /// Context for querying the MySQL database
    /// </summary>
    public MySQLDbContext DbContext { get; }

    /// <summary>
    /// Main constructor
    /// </summary>
    /// <param name="context">Database context</param>
    public MySQLSearchRepository(MySQLDbContext context)
    {
        DbContext = context;
    }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="searchCriteria">Criteria sent by the client</param>
    /// <returns>A IQueryable<Book> object</returns>
    public abstract IQueryable<Book> GetOrderedBooksRequest(SearchCriteriaDTO searchCriteria);

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

    /// <summary>
    /// Indicates if the database is available or not
    /// </summary>
    /// <returns>A boolean value</returns>
    public bool IsDatabaseAvailable()
    {
        if (DbContext is not null)
        {
            return DbContext.IsDatabaseAvailable();
        }

        return false;
    }
}