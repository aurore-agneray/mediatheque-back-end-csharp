namespace mediatheque_back_csharp.DTOs.SearchDTOs
{
    /// <summary>
    /// Object retrieved for the Author while searching for books
    /// </summary>
    public class AuthorResultDTO
    {
        /// <summary>
        /// First name + last name (generated into the database)
        /// </summary>
        public String? CompleteName { get; set; }
    }
}