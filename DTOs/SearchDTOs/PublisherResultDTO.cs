namespace mediatheque_back_csharp.DTOs.SearchDTOs
{
    /// <summary>
    /// Object retrieved for the Publisher while searching for books
    /// </summary>
    public class PublisherResultDTO
    {
        /// <summary>
        /// Name of the publishing house
        /// </summary>
        public string? PublishingHouse { get; set; }
    }
}