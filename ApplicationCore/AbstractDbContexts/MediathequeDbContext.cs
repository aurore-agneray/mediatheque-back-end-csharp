using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.AbstractDbContexts;

/// <summary>
/// Allows the use of several kinds of databases contexts
/// </summary>
/// <remarks>
/// Constructor for the MediathequeDbContext
/// </remarks>
/// <param name="settings">
/// Contains the settings used for connecting to the database
/// </param>
public abstract class MediathequeDbContext<T>(DbContextOptions settings) : DbContext(settings), IMediathequeDbContextFields
    where T : class, IDatabaseSettings
{
    /// <summary>
    /// List of Authors from the database
    /// </summary>
    public virtual DbSet<Author> Authors { get; set; }

    /// <summary>
    /// List of Books from the database
    /// </summary>
    public virtual DbSet<Book> Books { get; set; }

    /// <summary>
    /// List of Editions from the database
    /// </summary>
    public virtual DbSet<Edition> Editions { get; set; }

    /// <summary>
    /// List of Formats from the database
    /// </summary>
    public virtual DbSet<Format> Formats { get; set; }

    /// <summary>
    /// List of Genres from the database
    /// </summary>
    public virtual DbSet<Genre> Genres { get; set; }

    /// <summary>
    /// List of Publishers from the database
    /// </summary>
    public virtual DbSet<Publisher> Publishers { get; set; }

    /// <summary>
    /// List of Series from the database
    /// </summary>
    public virtual DbSet<Series> Series { get; set; }

    /// <summary>
    /// Indicates if the database is available or not
    /// </summary>
    /// <returns>A boolean value</returns>
    public abstract bool IsDatabaseAvailable();
}