using ApplicationCore.AbstractDbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MySQL;

/// <summary>
/// Global context for the connection to the "Mediatheque" MySQL database
/// </summary>
/// <remarks>
/// Constructor for the MySQLDbContext
/// </remarks>
/// <param name="settings">
/// Contains the settings used for connecting to the database
/// </param>
public class MySQLDbContext(DbContextOptions settings) : MediathequeDbContext<MySQLDatabaseSettings>(settings)
{
    /// <summary>
    /// Indicates if the database is available or not
    /// </summary>
    /// <returns>A boolean value</returns>
    public override bool IsDatabaseAvailable()
    {
        return Database.CanConnect();
    }
}