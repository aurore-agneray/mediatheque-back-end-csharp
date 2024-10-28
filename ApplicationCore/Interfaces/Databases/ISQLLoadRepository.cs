using ApplicationCore.Pocos;

namespace ApplicationCore.Interfaces.Databases;

/// <summary>
/// Defines the structure of SQL Repositories classes used for loading the forms
/// </summary>
public interface ISQLLoadRepository<out T> : ISQLRepository<T> 
    where T : IMediathequeDbContextFields
{
    /// <summary>
    /// Converts the DbSet with the Genres into a IQueryable
    /// </summary>
    public IQueryable<Genre> GetGenres();

    /// <summary>
    /// Converts the DbSet with the Publishers into a IQueryable
    /// </summary>
    public IQueryable<Publisher> GetPublishers();

    /// <summary>
    /// Converts the DbSet with the Formats into a IQueryable
    /// </summary>
    public IQueryable<Format> GetFormats();
}