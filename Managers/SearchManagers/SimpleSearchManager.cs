using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Extensions;

namespace mediatheque_back_csharp.Managers.SearchManagers;

/// <summary>
/// Methods for preparing the data sent by the SimpleSearchController
/// </summary>
public class SimpleSearchManager
{
    /// <summary>
    /// Order the given list by volumes' alphanumerical ascending order
    /// </summary>
    /// <param name="editions">List of EditionResultDTO objects</param>
    /// <returns>Ordered list of EditionResultDTO objects</returns>
    public List<EditionResultDTO> OrderEditionsByVolume(IEnumerable<EditionResultDTO> editions)
    {
        return editions.OrderBy(item => item.Volume.ExtractPrefix())
                       .ThenBy(item => item.Volume.ExtractNumber())
                       .ToList();
    }
}