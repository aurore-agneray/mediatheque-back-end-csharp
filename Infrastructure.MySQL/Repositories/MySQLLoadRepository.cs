using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;

namespace Infrastructure.MySQL.Repositories;

/// <summary>
/// Methods for loading the values used into the dropdown lists
/// </summary>
/// <remarks>
/// Main constructor
/// </remarks>
/// <param name="context">Database context</param>
public class MySQLLoadRepository(MySQLDbContext context) : MySQLRepository(context), ISQLLoadRepository<MySQLDbContext>
{
    /// <summary>
    /// Returns the formats from the DbContext into a IQueryable
    /// </summary>
    public virtual IQueryable<Format> GetFormats()
    {
        return DbContext.Formats;
    }

    /// <summary>
    /// Returns the genres from the DbContext into a IQueryable
    /// </summary>
    public virtual IQueryable<Genre> GetGenres()
    {
        return DbContext.Genres;
    }

    /// <summary>
    /// Returns the publishers from the DbContext into a IQueryable
    /// </summary>
    public virtual IQueryable<Publisher> GetPublishers()
    {
        return DbContext.Publishers;
    }
}