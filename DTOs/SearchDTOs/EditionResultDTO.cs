namespace mediatheque_back_csharp.DTOs.SearchDTOs
{
    /// <summary>
    /// Object retrieved for the Edition while searching for books
    /// </summary>
    public class EditionResultDTO
    {
        /// <summary>
        /// ID (primary key)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ISBN
        /// </summary>
        public string Isbn { get; set; } = null!;

        /// <summary>
        /// Ark id
        /// </summary>
        public string? ArkId { get; set; }

        /// <summary>
        /// Subtitle
        /// </summary>
        public string? Subtitle { get; set; }

        /// <summary>
        /// Publication date
        /// </summary>
        public DateTime? PublicationDate { get; set; }

        /// <summary>
        /// Publication date exclusively returned by the Bnf's API
        /// </summary>
        public string? PublicationDateBnf { get; set; }

        /// <summary>
        /// Publication year
        /// </summary>
        public int? PublicationYear { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        public string? Volume { get; set; }

        /// <summary>
        /// Summary
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// Book ID
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Format DTO
        /// </summary>
        public FormatResultDTO? Format { get; set; }

        /// <summary>
        /// Series DTO
        /// </summary>
        public SeriesResultDTO? Series { get; set; }

        /// <summary>
        /// Publisher DTO
        /// </summary>
        public PublisherResultDTO? Publisher { get; set; }
    }
}