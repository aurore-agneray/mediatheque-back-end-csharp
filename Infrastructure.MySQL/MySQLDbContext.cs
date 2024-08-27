using ApplicationCore.Interfaces;
using ApplicationCore.Pocos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.MySQL
{
    /// <summary>
    /// Global context for the connection to the "Mediatheque" MySQL database
    /// </summary>
    public class MySQLDbContext : DbContext, IMediathequeDbContext
    {
        /// <summary>
        /// Settings used for connecting to the database
        /// </summary>
        private readonly IOptions<MySQLDatabaseSettings> _appSettings;

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
        /// Constructor for the MediathequeDbContext
        /// </summary>
        /// <param name="settings">
        /// Contains the settings used for connecting to the database
        /// </param>
        public MySQLDbContext(IOptions<MySQLDatabaseSettings> settings)
        {
            _appSettings = settings;
        }

        /// <summary>
        /// Configures the Db Context
        /// </summary>
        /// <param name="optionsBuilder">
        /// Tool used for binding the context with the database
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Occurs when the connection string doesn't exist into the appsettings.json file
        /// </exception>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = _appSettings.Value.DbConnectionString;

            if (connectionString == null)
            {
                throw new ArgumentNullException("Please insert the Connection String into the appsettings.json file !");
            }

            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );
        }

        /// <summary>
        /// Indicates if the database is available or not
        /// </summary>
        /// <returns>A boolean value</returns>
        public bool IsDatabaseAvailable()
        {
            return Database.CanConnect();
        }
    }
}