namespace mediatheque_back_csharp.DTOs.SearchDTOs;

/// <summary>
/// Object retrieved while searching for books.
/// Contains all data about the Book and its Editions
/// </summary>
public class SearchResultDTO
{
    /// <summary>
    /// Constructor with one BookDTO parameter.
    /// Generates all sub-DTOs
    /// </summary>
    /// <param name="book">A BookDTO object</param>
    public SearchResultDTO(BookResultDTO book)
    {
        if (book != null)
        {
            BookId = book.Id;
            Book = book;
        }
    }

    /// <summary>
    /// ID of the book
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Object representing the book
    /// </summary>
    public BookResultDTO? Book { get; set; }

    /// <summary>
    /// Objects representing the editions of the book
    /// They are grouped by series' name ! (the key of string type)
    /// </summary>
    public Dictionary<string, List<EditionResultDTO>> Editions { get; set; }
}