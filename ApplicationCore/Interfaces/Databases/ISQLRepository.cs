namespace ApplicationCore.Interfaces.Databases;

/// <summary>
/// Defines the common properties and methods of all SQL repositories
/// </summary>
public interface ISQLRepository<out T> where T : IMediathequeDbContextFields
{
    /// <summary>
    /// Context for querying a SQL database
    /// </summary>
    public T DbContext { get; }

    /// <summary>
    /// Indicates if the database is available or not
    /// </summary>
    /// <returns>A boolean value</returns>
    public bool IsDatabaseAvailable();
}