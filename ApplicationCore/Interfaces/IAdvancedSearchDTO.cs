using ApplicationCore.DTOs.SearchDTOs;

namespace ApplicationCore.Interfaces;

/// <summary>
/// Arguments received for an advanced search
/// </summary>
public interface IAdvancedSearchDTO
{
    /// <summary>
    /// Object containing the criteria for the advanced search (title, author, genre, isbn, ...)
    /// </summary>
    public AdvancedSearchCriteriaDTO? AdvancedCriteria { get; set; }
}