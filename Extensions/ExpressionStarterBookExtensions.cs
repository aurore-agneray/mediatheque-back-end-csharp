using LinqKit;
using mediatheque_back_csharp.Pocos;

namespace mediatheque_back_csharp.Extensions;

/// <summary>
/// Describes the extensions for the class ExpressionStarter<Book>
/// </summary>
public static class ExpressionStarterBookExtensions
{
    /// <summary>
    /// Completes the concerned Expression with the condition on the publication date
    /// </summary>
    /// <param name="expression">ExpressionStarter<Book> object</param>
    /// <param name="op">Operator ("=", "<", ">")</param>
    /// <param name="criterionDate">Date used for comparison with the database</param>
    /// <returns>Returns the completed expression</returns>
    /// <exception cref="ArgumentException">Occured when the operator has an inappropriate value</exception>
    public static ExpressionStarter<Book> AddPublicationDateCondition(this ExpressionStarter<Book> expression, string op, DateTime criterionDate)
    {
        switch (op)
        {
            case "=":
                expression = expression.Or(book => 
                    book.Editions.Any(ed => 
                        (ed.PublicationDate.HasValue && ed.PublicationDate.Value.Date == criterionDate.Date)
                        || (ed.PublicationYear.HasValue && ed.PublicationYear.Value == criterionDate.Year)
                    ));
                break;
            case "<":
                expression = expression.Or(book => 
                    book.Editions.Any(ed => 
                        (ed.PublicationDate.HasValue && ed.PublicationDate.Value.Date < criterionDate.Date)
                        || (ed.PublicationYear.HasValue && ed.PublicationYear.Value < criterionDate.Year)
                    ));
                break;
            case ">":
                expression = expression.Or(book => 
                    book.Editions.Any(ed => 
                        (ed.PublicationDate.HasValue && ed.PublicationDate.Value.Date > criterionDate.Date)
                        || (ed.PublicationYear.HasValue && ed.PublicationYear.Value > criterionDate.Year)
                    ));
                break;
            default:
                throw new ArgumentException("Invalid operator for dates' comparison", nameof(op));
        }

        return expression;
    }
}