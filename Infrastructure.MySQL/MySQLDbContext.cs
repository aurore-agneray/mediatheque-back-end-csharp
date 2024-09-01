using ApplicationCore.AbstractClasses;
using ApplicationCore.DatabasesSettings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MySQL;

/// <summary>
/// Global context for the connection to the "Mediatheque" MySQL database
/// </summary>
public class MySQLDbContext : MediathequeDbContext<MySQLDatabaseSettings>
{
    /// <summary>
    /// Constructor for the MySQLDbContext
    /// </summary>
    /// <param name="settings">
    /// Contains the settings used for connecting to the database
    /// </param>
    public MySQLDbContext(DbContextOptions settings) : base(settings)
    {
    }

    /// <summary>
    /// Indicates if the database is available or not
    /// </summary>
    /// <returns>A boolean value</returns>
    public override bool IsDatabaseAvailable()
    {
        return Database.CanConnect();
    }
}