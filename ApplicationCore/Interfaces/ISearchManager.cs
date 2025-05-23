﻿using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Enums;
using System.Resources;

namespace ApplicationCore.Interfaces;

/// <summary>
/// A common interface for all classes of type "SearcheManager".
/// Permits to abstract the affectation of search services
/// </summary>
/// <typeparam name="T">An interface which defines objects with several services</typeparam>
public interface ISearchManager<out T> where T : class, IAllSearchServices
{
    /// <summary>
    /// Gives access to the texts of the app
    /// </summary>
    public ResourceManager TextsManager { get; init; }

    /// <summary>
    /// Processes the search that can be of type "simple" or "advanced".
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <param name="searchType">Type of search</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    public Task<List<SearchResultDTO>> SearchForResults(SearchDTO searchCriteria, SearchTypeEnum searchType);
}