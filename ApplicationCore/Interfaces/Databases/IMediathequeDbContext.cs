using ApplicationCore.AbstractClasses;
using ApplicationCore.Pocos;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Interfaces.Databases;

public interface IMediathequeDbContext<T> where T : class, IDatabaseSettings
{
    /// <summary>
    /// Defines available complex requests used for retrieving books and editions
    /// </summary>
    public ISQLRequests<T, MediathequeDbContext<T>> ComplexRequests { get; }

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
    /// Indicates if the database is available or not
    /// </summary>
    /// <returns>A boolean value</returns>
    public bool IsDatabaseAvailable();
}