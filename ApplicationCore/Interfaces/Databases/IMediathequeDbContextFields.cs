using ApplicationCore.AbstractClasses;
using ApplicationCore.Pocos;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Interfaces.Databases;

/// <summary>
/// Indicates which lists have to be implemented into the db contexts of the app
/// </summary>
public interface IMediathequeDbContextFields
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
}