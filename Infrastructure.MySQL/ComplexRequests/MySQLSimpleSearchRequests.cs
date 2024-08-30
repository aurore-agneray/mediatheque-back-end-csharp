using ApplicationCore.DTOs.SearchDTOs.CriteriaDTOs;
using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Pocos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MySQL.ComplexRequests;

/// <summary>
/// Requests used for the MySQL simple search
/// </summary>
public class MySQLSimpleSearchRequests : ISQLRequests<MySQLDbContext>
{
    /// <summary>
    /// Context for querying the MySQL database
    /// </summary>
    public MySQLDbContext DbContext { get; }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="searchCriteria">Contains the criterion sent by the client</param>
    /// <returns>A IQueryable<Book> object</returns>
    public IQueryable<Book> GetOrderedBooksRequest(SearchCriteriaDTO searchCriteria)
    {
        if (string.IsNullOrEmpty(searchCriteria?.SimpleCriterion))
        {
            return new List<Book>().AsQueryable();
        }

        string criterion = searchCriteria?.SimpleCriterion;

        return (
            from boo in DbContext.Books
            join author in DbContext.Authors on boo.AuthorId equals author.Id
            join ed in DbContext.Editions on boo.Id equals ed.BookId
            join series in DbContext.Series on ed.SeriesId equals series.Id into seriesJoin
            from series in seriesJoin.DefaultIfEmpty() // Retrieves editions even if they have no series
            where boo.Title != null && boo.Title.Contains(criterion)
                || author.CompleteName != null && author.CompleteName.Contains(criterion)
                || ed.Isbn != null && ed.Isbn.Contains(criterion)
                || series.Name != null && series.Name.Contains(criterion)
            select boo
        )
        .Distinct()
        .OrderBy(b => b.Title);
    }
}