using AutoMapper;
using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Extensions;
using mediatheque_back_csharp.Pocos;
using System.Linq.Expressions;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the SimpleSearchController
/// </summary>
public class SimpleSearchManager
{
    /// <summary>
    /// Transforms the POCOs into DTOs
    /// </summary>
    protected readonly IMapper _mapper;

    /// <summary>
    /// Constructor of the SimpleSearchManager class
    /// </summary>
    /// <param name="mapper">Given AutoMapper</param>
    public SimpleSearchManager(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Order the given list by volumes' alphanumerical ascending order
    /// </summary>
    /// <param name="editions">List of EditionResultDTO objects</param>
    /// <returns>Ordered list of EditionResultDTO objects</returns>
    private List<EditionResultDTO> OrderEditionsByVolume(IEnumerable<EditionResultDTO> editions)
    {
        return editions.OrderBy(item => item.Volume.ExtractPrefix())
                       .ThenBy(item => item.Volume.ExtractNumber())
                       .ToList();
    }

    /// <summary>
    /// Groups the editions of the given list into a dictionary 
    /// where the keys are the series' names
    /// </summary>
    /// <param name="editions">List of EditionResultDTOs objects</param>
    /// <returns>Returns a dictionary where the keys are the series' names
    /// and the elements are some lists containing editions</returns>
    private Dictionary<string, List<EditionResultDTO>> GroupEditionsBySeriesName(IEnumerable<EditionResultDTO> editions)
    {
        if (editions == null || editions.Count() == 0)
        {
            return new Dictionary<string, List<EditionResultDTO>>();
        }

        return editions.GroupBy(ed =>
        {
            if (ed?.Series?.SeriesName != null)
            {
                return ed.Series.SeriesName;
            }

            return "0";

        }).ToDictionary(group => group.Key, group => group.ToList());
    }

    /// <summary>
    /// Returns the expression used for searching the books into the DbContext.
    /// </summary>
    /// <param name="criterion">Title, author name, ISBN or series' name</param>
    /// <returns>Returns an expression which receive a Book as a parameter
    /// and returns boolean expressions</returns>
    public Expression<Func<Book, bool>> GetSearchConditions(string criterion)
    {
        if (string.IsNullOrEmpty(criterion))
        {
            return book => true;
        }

        return book => (
            book.Title != null
            && book.Title.ToLower().Contains(criterion)
        )
        || (
            book.Author != null
            && book.Author.CompleteName != null
            && book.Author.CompleteName.ToLower().Contains(criterion)
        )
        || (
            book.Editions != null
            && book.Editions.Any(edition => edition.Isbn == criterion
                || (edition.Series != null && edition.Series.Name != null && edition.Series.Name.ToLower().Contains(criterion)))
        );
    }

    /// <summary>
    /// Constructs the final results list from the given books list
    /// </summary>
    /// <param name="books">Books sent by the database</param>
    /// <returns>List of SearchResultDTOs (BookId + Book object + Editions grouped by series' name)</returns>
    public IEnumerable<SearchResultDTO> GetSimpleSearchResults(List<Book> books)
    {
        if (books == null) { 
            return Enumerable.Empty<SearchResultDTO>(); 
        }

        // Maps the books and the editions separately
        // Orders the book by title's ascending alphabetical order
        var bookResultDtos = _mapper.Map<List<Book>, List<BookResultDTO>>(books)
                                    .OrderBy(book => book.Title);

        var editionResultDtos = _mapper.Map<IEnumerable<Edition>, List<EditionResultDTO>>(
            books.SelectMany(res => res.Editions)
        );

        return bookResultDtos.Select(bookDto =>
        {
            // Binds each book to its editions and orders the editions by volume
            var editionsForABook = OrderEditionsByVolume(
                editionResultDtos.Where(edition => edition.BookId == bookDto.Id)
            );

            return new SearchResultDTO(bookDto)
            {
                // Editions have to be grouped by series' name
                Editions = GroupEditionsBySeriesName(editionsForABook)
            };
        });
    }
}