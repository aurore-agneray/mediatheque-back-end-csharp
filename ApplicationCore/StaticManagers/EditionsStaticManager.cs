using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Extensions;

namespace ApplicationCore.StaticManagers;

/// <summary>
/// Gives static methods for processing lists of editions
/// </summary>
public static class EditionsStaticManager
{
    /// <summary>
    /// Order the given list by volumes' alphanumerical ascending order
    /// </summary>
    /// <param name="editions">List of EditionResultDTO objects</param>
    /// <returns>Ordered list of EditionResultDTO objects</returns>
    public static List<EditionResultDTO> OrderEditionsByVolume(IEnumerable<EditionResultDTO> editions)
    {
        return editions.OrderBy(item => item?.Volume != null ? item.Volume.ExtractPrefixWithoutNumbers() : string.Empty)
                       .ThenBy(item => item?.Volume != null ? item.Volume.ExtractNumber() : 0)
                       .ToList();
    }

    /// <summary>
    /// Groups the editions of the given list into a dictionary 
    /// where the keys are the series' names
    /// </summary>
    /// <param name="editions">List of EditionResultDTOs objects</param>
    /// <returns>Returns a dictionary where the keys are the series' names
    /// and the elements are some lists containing editions</returns>
    public static Dictionary<string, List<EditionResultDTO>> GroupEditionsBySeriesName(IEnumerable<EditionResultDTO> editions)
    {
        if (editions == null || !editions.Any())
        {
            return [];
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