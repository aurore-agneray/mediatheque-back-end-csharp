using mediatheque_back_csharp.Dtos;
using mediatheque_back_csharp.Interfaces;

namespace mediatheque_back_csharp.DTOs.SearchDTOs
{
    /// <summary>
    /// Object retrieved for the Book while searching for books
    /// </summary>
    public class BookResultDTO : IIdentified
    {
        /// <summary>
        /// Constructor of the BookResultDTO
        /// </summary>
        /// <param name="book">A BookDTO object</param>
        public BookResultDTO(BookDTO book)
        {
            if (book != null)
            {
                Id = book.Id;
                Title = book.Title;
                StaffComment = book.StaffComment;
                Author = new AuthorResultDTO() { CompleteName = book.Author?.CompleteName };
                Genre = new GenreResultDTO() { GenreName = book.Genre?.Name };
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Staff comment
        /// </summary>
        public string? StaffComment { get; set; }

        /// <summary>
        /// Author Result DTO
        /// </summary>
        public AuthorResultDTO? Author { get; set; }

        /// <summary>
        /// Genre Result DTO
        /// </summary>
        public GenreResultDTO? Genre { get; set; }
    }
}