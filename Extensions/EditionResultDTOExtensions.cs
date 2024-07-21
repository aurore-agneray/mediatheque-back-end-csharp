using mediatheque_back_csharp.DTOs.SearchDTOs;

namespace mediatheque_back_csharp.Extensions;

/// <summary>
/// Extensions methods for the class EditionResultDTO
/// </summary>
public static class EditionResultDTOExtensions {

    /// <summary>
    /// Groups the editions of the given list into a dictionary 
    /// where the keys are the series' names
    /// </summary>
    /// <param name="editions">List of EditionResultDTOs objects</param>
    /// <returns>Returns a dictionary where the keys are the series' names
    /// and the elements are some lists containing editions</returns>
    public static Dictionary<string, List<EditionResultDTO>> GroupElementsBySeriesName(this IEnumerable<EditionResultDTO> editions)
    {
        if (editions == null || editions.Count() == 0)
        {
            return new Dictionary<string, List<EditionResultDTO>>();
        }

        return editions.GroupBy(ed =>
        {
            if (!string.IsNullOrEmpty(ed?.Series?.SeriesName))
            {
                return ed.Series.SeriesName;
            }

            return "0";

        }).ToDictionary(
            group => group.Key, 
            group => group.ToList()
        );
    }
}