using AutoMapper;
using LinqKit;
using mediatheque_back_csharp.Database;
using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Extensions;
using mediatheque_back_csharp.Pocos;
using Microsoft.EntityFrameworkCore;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the AdvancedSearchController
/// </summary>
public class AdvancedSearchManager : SearchManager
{
    /// <summary>
    /// Constructor of the AdvancedSearchManager class
    /// </summary>
    /// <param name="context">HTTP Context</param>
    /// <param name="mapper">Given AutoMapper</param>
    public AdvancedSearchManager(MediathequeDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="criteria">Criteria sent by the client</param>
    /// <returns>A IQueryable<Book> object</returns>
    private IQueryable<Book> GetOrderedBooksRequest(SearchArgsDTO criteria)
    {
        if (criteria == null)
        {
            return default;
        }
        
        DateTime criterionDate;

        IQueryable<Book> query = (
            from boo in _context.Books
            join genre in _context.Genres on boo.GenreId equals genre.Id
            join author in _context.Authors on boo.AuthorId equals author.Id
            join ed in _context.Editions on boo.Id equals ed.BookId
            join publisher in _context.Publishers on ed.PublisherId equals publisher.Id
            join format in _context.Formats on ed.FormatId equals format.Id
            join series in _context.Series on ed.SeriesId equals series.Id into seriesJoin
            from series in seriesJoin.DefaultIfEmpty() // Retrieves editions even if they have no series
            select boo
        )
        .Distinct()
        .OrderBy(b => b.Title);

        var expression = PredicateBuilder.New<Book>(false);

        if (!string.IsNullOrEmpty(criteria?.Title))
        {
            expression = expression.Or(book =>
                book.Title != null && book.Title.Contains(criteria.Title)
            );
        }

        if (!string.IsNullOrEmpty(criteria?.Isbn))
        {
            expression = expression.Or(book =>
                book.Editions.Any(ed => ed.Isbn == criteria.Isbn)
            );
        }

        if (criteria?.Publisher?.Id != null)
        {
            expression = expression.Or(book =>
                book.Editions.Any(ed => ed.PublisherId == criteria.Publisher.Id)
            );
        }

        if (!string.IsNullOrEmpty(criteria?.Series))
        {
            expression = expression.Or(book =>
                book.Editions.Any(ed =>
                    ed.Series != null && ed.Series.Name != null && ed.Series.Name.Contains(criteria.Series)
                )
            );
        }

        if (criteria?.Format?.Id != null)
        {
            expression = expression.Or(book =>
                book.Editions.Any(ed => ed.FormatId == criteria.Format.Id)
            );
        }

        if (!string.IsNullOrEmpty(criteria?.PubDate?.Criterion) 
            && !string.IsNullOrEmpty(criteria?.PubDate?.Operator)
            && DateTime.TryParse(criteria.PubDate.Criterion, out criterionDate)) 
        {
            expression = expression.AddPublicationDateCondition(criteria.PubDate.Operator, criterionDate);
        }

        if (!string.IsNullOrEmpty(criteria?.Author))
        {
            expression = expression.Or(book =>
                book.Author != null && book.Author.CompleteName != null && book.Author.CompleteName.Contains(criteria.Author)
            );
        }

        if (criteria?.Genre?.Id != null)
        {
            expression = expression.Or(book => book.Genre != null && book.Genre.Id == criteria.Genre.Id);
        }

        return query.Where(expression);
    }

    /// <summary>
    /// Processes the advanced search
    /// </summary>
    /// <param name="criteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    public override async Task<List<SearchResultDTO>> SearchForResults(SearchArgsDTO criteria)
    {
        var query = GetOrderedBooksRequest(criteria);

        if (query != null)
        {
            var asyncList = await query.ToListAsync();
            var dtos = _mapper.Map<List<BookResultDTO>>(asyncList);
            return dtos.Select(dto => new SearchResultDTO(dto)).ToList();
        }

        return new List<SearchResultDTO>();
    }
}