using ApplicationCore.DTOs.SearchDTOs.CriteriaDTOs;
using ApplicationCore.Pocos;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Interfaces;

/// <summary>
/// Defines the structure of SQLRequests classes
/// </summary>
public interface ISQLRequests<T> where T : DbContext
{
    /// <summary>
    /// Context for querying a SQL database
    /// </summary>
    public T DbContext { get; }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="searchCriteria">Contains the criterion sent by the client</param>
    /// <returns>A IQueryable<Book> object</returns>
    public IQueryable<Book> GetOrderedBooksRequest(SearchCriteriaDTO searchCriteria);
}
