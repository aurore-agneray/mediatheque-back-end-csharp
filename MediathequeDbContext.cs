using mediatheque_back_csharp.Pocos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace mediatheque_back_csharp
{
    /// <summary>
    /// Global context for the connection to the "Mediatheque" MySQL database
    /// </summary>
    public class MediathequeDbContext : DbContext
    {
        /// <summary>
        /// Settings entered into the appsettings.json file, into the "MySettings" section
        /// </summary>
        private readonly IOptions<MySettingsModel> _appSettings;

        /// <summary>
        /// Constructor for the MediathequeDbContext
        /// </summary>
        /// <param name="settings">
        /// Object representing the settings entered 
        /// into the appsettings.json file, into the "MySettings" section.
        /// Instanciated into the Program.cs file
        /// </param>
        public MediathequeDbContext(IOptions<MySettingsModel> settings)
        {
            _appSettings = settings;
        }

        /// <summary>
        /// List of Authors from the database
        /// </summary>
        public DbSet<Author> Authors { get; set; }

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
    }
}
