using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Pocos;
using Infrastructure.MySQL.Repositories;
using LinqKit;

namespace Infrastructure.MySQL.ComplexRequests;

/// <summary>
/// Requests used for the MySQL advanced search
/// </summary>
/// <remarks>
/// Main constructor
/// </remarks>
/// <param name="context">Database context</param>
public class MySQLAdvancedSearchRepository(MySQLDbContext context) : MySQLSearchRepository(context)
{
    /// <summary>
    /// Completes the concerned Expression with the condition on the publication date
    /// </summary>
    /// <param name="expression">ExpressionStarter<Book> object</param>
    /// <param name="op">Operator ("=", "<", ">")</param>
    /// <param name="criterionDate">Date used for comparison with the database</param>
    /// <returns>Returns the completed expression</returns>
    /// <exception cref="ArgumentException">Occured when the operator has an inappropriate value</exception>
    private static ExpressionStarter<Book> AddPublicationDateCondition(ExpressionStarter<Book> expression, string op, DateTime criterionDate)
    {
        /* WARNING : I'd like to factorize more this part but it seems impossible because of the request translation */
        expression = op switch
        {
            "=" => (ExpressionStarter<Book>)expression.Or(book =>
                    book.Editions.Any(ed =>
                        ed.PublicationDate.HasValue && ed.PublicationDate.Value.Date == criterionDate.Date
                        || ed.PublicationYear.HasValue && ed.PublicationYear.Value == criterionDate.Year
                    )
                ),
            "<" => (ExpressionStarter<Book>)expression.Or(book =>
                    book.Editions.Any(ed =>
                        ed.PublicationDate.HasValue && ed.PublicationDate.Value.Date < criterionDate.Date
                        || ed.PublicationYear.HasValue && ed.PublicationYear.Value < criterionDate.Year
                    )
                ),
            ">" => (ExpressionStarter<Book>)expression.Or(book =>
                    book.Editions.Any(ed =>
                        ed.PublicationDate.HasValue && ed.PublicationDate.Value.Date > criterionDate.Date
                        || ed.PublicationYear.HasValue && ed.PublicationYear.Value > criterionDate.Year
                    )
                ),
            _ => throw new ArgumentException("Invalid operator for dates' comparison", nameof(op)),
        };
        
        return expression;
    }

    /// <summary>
    /// Completes the concerned Expression with the condition on the book object and its properties,
    /// except the editions
    /// </summary>
    /// <param name="expression">ExpressionStarter<Book> object</param>
    /// <param name="criteria">Criteria sent by the client</param>
    private static void AddBookConditions(ref ExpressionStarter<Book> expression, AdvancedSearchCriteriaDTO criteria)
    {

        if (!string.IsNullOrEmpty(criteria?.Title))
        {
            expression = expression.Or(book =>
                book.Title != null
                && book.Title.Contains(criteria.Title)
            );
        }

        if (!string.IsNullOrEmpty(criteria?.Author))
        {
            expression = expression.Or(book =>
                book.Author != null
                && book.Author.CompleteName != null
                && book.Author.CompleteName.Contains(criteria.Author)
            );
        }

        if (criteria?.Genre?.Id != null)
        {
            expression = expression.Or(book => book.Genre != null && book.Genre.Id == criteria.Genre.Id);
        }
    }

    /// <summary>
    /// Completes the concerned Expression with the condition on the editions and their properties
    /// </summary>
    /// <param name="expression">ExpressionStarter<Book> object</param>
    /// <param name="criteria">Criteria sent by the client</param>
    private static void AddEditionsConditions(ref ExpressionStarter<Book> expression, AdvancedSearchCriteriaDTO criteria)
    {
        if (!string.IsNullOrEmpty(criteria?.Isbn))
        {
            expression = expression.Or(book =>
                book.Editions.Any(ed =>
                    ed.Isbn != null
                    && ed.Isbn.Replace("-", string.Empty) == criteria.Isbn.Replace("-", string.Empty)
                )
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
                    ed.Series != null
                    && ed.Series.Name != null
                    && ed.Series.Name.Contains(criteria.Series)
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
            && DateTime.TryParse(criteria.PubDate.Criterion, out DateTime criterionDate))
        {
            expression = AddPublicationDateCondition(expression, criteria.PubDate.Operator, criterionDate);
        }
    }

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="searchCriteria">Criteria sent by the client</param>
    /// <returns>A IQueryable<Book> object or null</returns>
    public override IQueryable<Book>? GetOrderedBooksRequest<IAdvancedSearchDTO>(IAdvancedSearchDTO searchCriteria)
    {
        AdvancedSearchDTO? criteriaDto = null;

        if (searchCriteria is IAdvancedSearchDTO idto)
        {
            criteriaDto = searchCriteria as AdvancedSearchDTO;
        }
        else
        {
            return null;
        }

        if (criteriaDto?.AdvancedCriteria is null)
        {
            return null;
        }

        IQueryable<Book> query = (
            from boo in DbContext.Books
            join genre in DbContext.Genres on boo.GenreId equals genre.Id
            join author in DbContext.Authors on boo.AuthorId equals author.Id
            join ed in DbContext.Editions on boo.Id equals ed.BookId
            join publisher in DbContext.Publishers on ed.PublisherId equals publisher.Id
            join format in DbContext.Formats on ed.FormatId equals format.Id
            join series in DbContext.Series on ed.SeriesId equals series.Id into seriesJoin
            from series in seriesJoin.DefaultIfEmpty() // Retrieves editions even if they have no series
            select boo
        )
        .Distinct()
        .OrderBy(b => b.Title);

        var expression = PredicateBuilder.New<Book>(false);

        AddBookConditions(ref expression, criteriaDto.AdvancedCriteria);
        AddEditionsConditions(ref expression, criteriaDto.AdvancedCriteria);

        return query.Where(expression);
    }
}