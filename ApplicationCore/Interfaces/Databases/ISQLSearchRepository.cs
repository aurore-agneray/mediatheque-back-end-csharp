﻿using ApplicationCore.Interfaces.DTOs;
using ApplicationCore.Pocos;

namespace ApplicationCore.Interfaces.Databases;

/// <summary>
/// Defines the structure of SQL Repositories classes used for searching data
/// </summary>
public interface ISQLSearchRepository<out T> : ISQLRepository<T>
    where T : IMediathequeDbContextFields
{
    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the books from the database,
    /// ordered by the title
    /// </summary>
    /// <param name="searchCriteria">Contains the criterion sent by the client</param>
    /// <returns>A IQueryable<Book> object or null</returns>
    public IQueryable<Book>? GetOrderedBooksRequest<U>(U searchCriteria) where U : class, ISearchDTO;

    /// <summary>
    /// Generate the IQueryable object dedicated to 
    /// retrieve the editions from the database
    /// </summary>
    /// <param name="bookIds">List of the IDs of the concerned books</param>
    /// <returns>A IQueryable<Edition> object</returns>
    public IQueryable<Edition> GetEditionsForSeveralBooksRequest(int[] bookIds);
}