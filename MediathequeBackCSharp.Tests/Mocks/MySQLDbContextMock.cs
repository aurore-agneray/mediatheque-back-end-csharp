using ApplicationCore.Interfaces.Entities;
using ApplicationCore.Pocos;
using Infrastructure.MySQL;
using MediathequeBackCSharp.Tests.AsyncMockingConfiguration;
using MediathequeBackCSharp.Tests.DataSets;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace MediathequeBackCSharp.Tests.Mocks;

/// <summary>
/// Used for mimicking the behaviour of the MySQLDbContext
/// </summary>
internal static class MySQLDbContextMock
{
    private static Mock<MySQLDbContext> GetMockWithoutData()
    {
        var options = new DbContextOptionsBuilder<MySQLDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;

        return new Mock<MySQLDbContext>(options);
    }

    private static FakeAsyncDbSet<T>? MockArrayAsDbSet<T>(T[] myArray) where T : class, IIdentified
    {
        if (myArray == null)
        {
            throw new ArgumentNullException("The given array is null, so it can't be mocked !");
        }

        //var queryable = myArray.AsQueryable();
        //var mockSet = queryable.BuildMockDbSet();

        //mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        //mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        //mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        //mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        var mockSet = new FakeAsyncDbSet<T>();

        for (int i = 0; i < myArray.Length; i++)
        {
            mockSet.List.Add(myArray[i]);
        }

        return mockSet;
    }

    internal static Mock<MySQLDbContext> GetMockForTestingLoadController()
    {
        var mock = GetMockWithoutData();

        var genres = LoadDataSets.GetUnorderedGenres().AsQueryable().BuildMockDbSet();
        var publishers = LoadDataSets.GetUnorderedPublishers().AsQueryable().BuildMockDbSet();
        var formats = LoadDataSets.GetUnorderedFormats().AsQueryable().BuildMockDbSet();

        mock.Setup<DbSet<Genre>>(m => m.Genres).Returns(genres.Object);
        mock.Setup<DbSet<Publisher>>(m => m.Publishers).Returns(publishers.Object);
        mock.Setup<DbSet<Format>>(m => m.Formats).Returns(formats.Object);
        mock.Setup(m => m.IsDatabaseAvailable()).Returns(() => true);

        return mock;
    }

    internal static Mock<MySQLDbContext> GetMockForTestingSearchController()
    {
        var mock = GetMockWithoutData();

        var genres = MockArrayAsDbSet<Genre>(SearchDataSets.Genres);
        var publishers = MockArrayAsDbSet<Publisher>(SearchDataSets.Publishers);
        var formats = MockArrayAsDbSet<Format>(SearchDataSets.Formats);
        var authors = MockArrayAsDbSet<Author>(SearchDataSets.Authors);
        var series = MockArrayAsDbSet<Series>(SearchDataSets.Series);
        var books = MockArrayAsDbSet<Book>(SearchDataSets.Books);
        var editions = MockArrayAsDbSet<Edition>(SearchDataSets.Editions);

        mock.Setup<DbSet<Genre>>(m => m.Genres).Returns(genres);
        mock.Setup<DbSet<Publisher>>(m => m.Publishers).Returns(publishers);
        mock.Setup<DbSet<Format>>(m => m.Formats).Returns(formats);
        mock.Setup<DbSet<Author>>(m => m.Authors).Returns(authors);
        mock.Setup<DbSet<Series>>(m => m.Series).Returns(series);
        mock.Setup<DbSet<Book>>(m => m.Books).Returns(books);
        mock.Setup<DbSet<Edition>>(m => m.Editions).Returns(editions);

        //var genres = SearchDataSets.Genres.AsQueryable().BuildMockDbSet();
        //var publishers = SearchDataSets.Publishers.AsQueryable().BuildMockDbSet();
        //var formats = SearchDataSets.Formats.AsQueryable().BuildMockDbSet();
        //var authors = SearchDataSets.Authors.AsQueryable().BuildMockDbSet();
        //var series = SearchDataSets.Series.AsQueryable().BuildMockDbSet();
        //var books = SearchDataSets.Books.AsQueryable().BuildMockDbSet();
        //var editions = SearchDataSets.Editions.AsQueryable().BuildMockDbSet();

        //mock.Setup<DbSet<Genre>>(m => m.Genres).Returns(genres.Object);
        //mock.Setup<DbSet<Publisher>>(m => m.Publishers).Returns(publishers.Object);
        //mock.Setup<DbSet<Format>>(m => m.Formats).Returns(formats.Object);
        //mock.Setup<DbSet<Author>>(m => m.Authors).Returns(authors.Object);
        //mock.Setup<DbSet<Series>>(m => m.Series).Returns(series.Object);
        //mock.Setup<DbSet<Book>>(m => m.Books).Returns(books.Object);
        //mock.Setup<DbSet<Edition>>(m => m.Editions).Returns(editions.Object);

        mock.Setup(m => m.IsDatabaseAvailable()).Returns(() => true);

        return mock;
    }
}