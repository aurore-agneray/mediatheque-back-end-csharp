using ApplicationCore.AbstractServices;
using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Interfaces.Databases;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediathequeBackCSharp.Texts;
using Microsoft.EntityFrameworkCore;
using System.Resources;

namespace MediathequeBackCSharp.Services.Abstracts;

/// <summary>
/// Abstract class that has to be inherited by simple and advanced MySQL search services
/// </summary>
public abstract class MySQLSearchService : SearchService
{
    /// <summary>
    /// Repository used for retrieving data from a particular database
    /// </summary>
    protected readonly ISQLRepository<IMediathequeDbContextFields>? _sqlRepository;

    /// <summary>
    /// Constructor of the MySQLSearchService class
    /// </summary>
    /// <param name="mapper">Given AutoMapper</param>
    /// <param name="textsManager">Texts manager</param>
    /// <param name="sqlRepo">Repository for collecting data from SQL databases</param>
    protected MySQLSearchService(IMapper mapper, ResourceManager textsManager, ISQLRepository<IMediathequeDbContextFields> sqlRepo)
        : base(mapper, textsManager)
    {
        _sqlRepository = sqlRepo;

        // Checks the availability of the repository and the database
        if (_sqlRepository == null)
        {
            throw new ArgumentException(nameof(_sqlRepository));
        }

        if (!_sqlRepository.IsDatabaseAvailable())
        {
            throw new Exception(TextsManager.GetString(TextsKeys.ERROR_DATABASE_CONNECTION) ?? string.Empty);
        }
    }

    /// <summary>
    /// Extracts the books and their editions from the concerned MySQL repository
    /// </summary>
    /// <param name="searchCriteria">Object containing the search criteria</param>
    /// <returns>List of some SearchResultsDTO objects</returns>
    protected override async Task<Tuple<List<BookResultDTO>, List<EditionResultDTO>>> ExtractDataFromRepository(SearchDTO searchCriteria)
    {
        List<BookResultDTO> booksList = [];
        List<EditionResultDTO> editionsList = [];

#pragma warning disable CS8602
        // Retrieves data --> the repository can't be null because an exception is thrown into the controller if that's the case
        var booksQuery = _sqlRepository.GetOrderedBooksRequest(searchCriteria);

#pragma warning restore CS8602

        // Completes the first list with the books
        if (booksQuery != null)
        {
            booksList = await booksQuery.Include(b => b.Author)
                                        .Include(b => b.Genre)
                                        .ProjectTo<BookResultDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync();
        }

        // Completes the second list with the editions
        if (booksList != null && booksList.Count == 0)
        {
            editionsList = await _sqlRepository.GetEditionsForSeveralBooksRequest(
                booksList.Select(bDto => bDto.Id).ToArray()
            )
            .Include(ed => ed.Format)
            .Include(ed => ed.Series)
            .Include(ed => ed.Publisher)
            .ProjectTo<EditionResultDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        return new(booksList ?? [], editionsList);
    }
}