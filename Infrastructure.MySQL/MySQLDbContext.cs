using ApplicationCore.AbstractClasses;
using ApplicationCore.DatabasesSettings;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.MySQL
{
    /// <summary>
    /// Global context for the connection to the "Mediatheque" MySQL database
    /// </summary>
    public class MySQLDbContext : MediathequeDbContext<MySQLDatabaseSettings>
    {
        /// <summary>
        /// Settings used for connecting to the database
        /// </summary>
        protected override IOptions<MySQLDatabaseSettings> DatabaseSettings { get; set; }

        /// <summary>
        /// Defines available complex requests used for retrieving books and editions
        /// </summary>
        public override ISQLRequests<MySQLDatabaseSettings, MediathequeDbContext<MySQLDatabaseSettings>> ComplexRequests { get; }

        /// <summary>
        /// List of Authors from the database
        /// </summary>
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// List of Books from the database
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// List of Editions from the database
        /// </summary>
        public DbSet<Edition> Editions { get; set; }

        /// <summary>
        /// List of Formats from the database
        /// </summary>
        public DbSet<Format> Formats { get; set; }

        /// <summary>
        /// List of Genres from the database
        /// </summary>
        public DbSet<Genre> Genres { get; set; }

        /// <summary>
        /// List of Publishers from the database
        /// </summary>
        public DbSet<Publisher> Publishers { get; set; }

        /// <summary>
        /// List of Series from the database
        /// </summary>
        public DbSet<Series> Series { get; set; }

        /// <summary>
        /// Constructor for the MySQLDbContext
        /// </summary>
        /// <param name="settings">
        /// Contains the settings used for connecting to the database
        /// </param>
        public MySQLDbContext(IOptions<MySQLDatabaseSettings> settings) : base(settings)
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
}