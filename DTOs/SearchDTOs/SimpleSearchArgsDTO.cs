using mediatheque_back_csharp.Constants;

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

        /// <summary>
        /// Indicates if the research will be done into the BnF (true) or the custom MySQL database (false)
        /// </summary>
        public bool ApiBnf { get; set; }

        /// <summary>
        /// Number of notices that the BnF's API will return for extracting data
        /// </summary>
        public int ApiBnfNoticesQty { get; set; } = BnfConsts.DEFAULT_NOTICES_NUMBER;
    }
}
