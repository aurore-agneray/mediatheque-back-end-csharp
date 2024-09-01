using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.AbstractClasses;

/// <summary>
/// Allows the use of several kinds of databases contexts
/// </summary>
public abstract class MediathequeDbContext<T> : DbContext, IMediathequeDbContextFields
    where T : class, IDatabaseSettings
{
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
    public MediathequeDbContext(DbContextOptions settings) : base(settings)
    {
    }

    /// <summary>
    /// Indicates if the database is available or not
    /// </summary>
    /// <returns>A boolean value</returns>
    public abstract bool IsDatabaseAvailable();
}