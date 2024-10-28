using ApplicationCore.Interfaces.Databases;

namespace Infrastructure.MySQL.Repositories;

/// <summary>
/// Contains common properties and methods for all MySQL repositories
/// </summary>
/// <remarks>
/// Main constructor
/// </remarks>
/// <param name="context">Database context</param>
public abstract class MySQLRepository(MySQLDbContext context) : ISQLRepository<MySQLDbContext>
{
    /// <summary>
    /// Context for querying the MySQL database
    /// </summary>
    public MySQLDbContext DbContext { get; } = context;

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