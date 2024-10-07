using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Pocos;
using Infrastructure.MySQL.Repositories;

namespace Infrastructure.MySQL.ComplexRequests;

/// <summary>
/// Requests used for the MySQL simple search
/// </summary>
/// <remarks>
/// Main constructor
/// </remarks>
/// <param name="context">Database context</param>
public class MySQLSimpleSearchRepository(MySQLDbContext context) : MySQLSearchRepository(context)
{
    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="searchCriteria">Contains the criterion sent by the client</param>
    /// <returns>A IQueryable<Book> object or null</returns>
    public override IQueryable<Book>? GetOrderedBooksRequest<ISimpleSearchDTO>(ISimpleSearchDTO searchCriteria)
    {
        SimpleSearchDTO? criteriaDto = null;

        if (searchCriteria is ISimpleSearchDTO idto)
        {
            criteriaDto = searchCriteria as SimpleSearchDTO;
        }
        else
        {
            return null;
        }

        if (string.IsNullOrEmpty(criteriaDto?.SimpleCriterion))
        {
            return default;
        }

        string criterion = criteriaDto.SimpleCriterion;

#pragma warning disable CS8602
/* The compiler considers that boo.Title and author.CompleteName can be null even if I checked for avoiding that */
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
#pragma warning restore CS8602
    }
}