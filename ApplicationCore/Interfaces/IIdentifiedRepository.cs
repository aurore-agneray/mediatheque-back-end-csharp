namespace ApplicationCore.Interfaces;

/// <summary>
/// Represents data access layer for the POCOs (or Entities) of a given database
/// </summary>
/// <typeparam name="T">A type with an ID property</typeparam>
public interface IIdentifiedRepository<T> where T : class, IIdentified
{
    /// <summary>
    /// Get CRUD request for the T entity.
    /// </summary>
    /// <returns>List of some IIdentified objects of the database</returns>
    public Task<IEnumerable<T>> GetAll();
}