using mediatheque_back_csharp.Constants;
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

            return BnfConsts.NO_SERIES_NAME;

        }).ToDictionary(
            group => group.Key, 
            group => group.ToList()
        );
    }

    /// <summary>
    /// Order the given list by volumes' alphanumerical ascending order
    /// </summary>
    /// <param name="editions">List of EditionResultDTO objects</param>
    /// <returns>Ordered list of EditionResultDTO objects</returns>
    public static List<EditionResultDTO> OrderElementsByVolume(this IEnumerable<EditionResultDTO> editions)
    {
        if (editions == null)
        {
            return new List<EditionResultDTO>();
        }

        return editions.OrderBy(item => item.Volume.ExtractPrefix())
                       .ThenBy(item => item.Volume.ExtractNumber())
                       .ToList();
    }
}