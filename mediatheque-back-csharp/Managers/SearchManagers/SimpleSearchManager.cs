using ApplicationCore.DTOs.SearchDTOs.CriteriaDTOs;
using ApplicationCore.Pocos;
using ApplicationCore.Texts;
using AutoMapper;
using Infrastructure.MySQL;
using System.Resources;
using static System.Linq.Queryable;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the SimpleSearchController
/// </summary>
public class SimpleSearchManager : SearchManager
{
    /// <summary>
    /// Constructor of the SimpleSearchManager class
    /// </summary>
    /// <param name="context">HTTP Context</param>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    public SimpleSearchManager(MySQLDbContext context, IMapper mapper, ResourceManager textsManager)
        : base(context, mapper, textsManager, Constants.SIMPLE_SEARCH_TYPE, TextsKeys.SIMPLE_SEARCH_TYPE)
    {
    }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="searchCriteria">Contains the criterion sent by the client</param>
    /// <returns>A IQueryable<Book> object</returns>
    protected override IQueryable<Book> GetOrderedBooksRequest(SearchCriteriaDTO searchCriteria) {

        if (string.IsNullOrEmpty(searchCriteria?.SimpleCriterion))
        {
            ThrowExceptionForMissingCriteria();
        }

        string criterion = searchCriteria?.SimpleCriterion;

        return (
            from boo in _context.Books
            join author in _context.Authors on boo.AuthorId equals author.Id
            join ed in _context.Editions on boo.Id equals ed.BookId
            join series in _context.Series on ed.SeriesId equals series.Id into seriesJoin
            from series in seriesJoin.DefaultIfEmpty() // Retrieves editions even if they have no series
            where (boo.Title != null && boo.Title.Contains(criterion))
                || (author.CompleteName != null && author.CompleteName.Contains(criterion))
                || (ed.Isbn != null && ed.Isbn.Contains(criterion))
                || (series.Name != null && series.Name.Contains(criterion))
            select boo
        )
        .Distinct()
        .OrderBy(b => b.Title);
    }
}