namespace mediatheque_back_csharp.DTOs.SearchDTOs
{
    /// <summary>
    /// Arguments received for the simple search
    /// </summary>
    public class SimpleSearchArgsDTO
    {
        /// <summary>
        /// Criterion (Book's title, author's name, series' name or ISBN)
        /// </summary>
        public string? Criterion { get; set; }
    }
}
