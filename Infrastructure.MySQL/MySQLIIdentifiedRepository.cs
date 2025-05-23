﻿using ApplicationCore.Interfaces.Databases;
using ApplicationCore.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MySQL;

/// <summary>
/// Retrieves the data from the concerned database
/// </summary>
/// <typeparam name="T">Type with an ID property</typeparam>
/// <remarks>
/// Constructor of the IIdentifiedController class
/// </remarks>
/// <param name="context">Given database context</param>
public class MySQLIIdentifiedRepository<T>(MySQLDbContext context) : IIdentifiedRepository<T> where T : class, IIdentified
{
    /// <summary>
    /// Context for connecting to the database
    /// </summary>
    //protected readonly MySQLDbContext _context = context;

    /// <summary>
    /// Get all CRUD request for the IIdentifieds objects
    /// </summary>
    /// <returns>List of some IIdentified objects of the database</returns>
    public async Task<IEnumerable<T>> GetAll()
    {
        //List<IIdentified> output = new List<IIdentified>(this._context.Authors);
        //output.AddRange(this._context.Books);
        //output.AddRange(this._context.Editions);
        //output.AddRange(this._context.Formats);
        //output.AddRange(this._context.Genres);
        //output.AddRange(this._context.Publishers);
        //output.AddRange(this._context.Series);

        //return output;
        return await context.Set<T>().ToListAsync();
    }
}
