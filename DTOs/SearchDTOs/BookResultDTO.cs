using mediatheque_back_csharp.Interfaces;

namespace mediatheque_back_csharp.DTOs.SearchDTOs
{
    /// <summary>
    /// Object retrieved for the Book while searching for books
    /// </summary>
    public class BookResultDTO : IIdentified
    {
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