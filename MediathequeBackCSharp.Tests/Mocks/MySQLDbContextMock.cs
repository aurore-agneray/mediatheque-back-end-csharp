using ApplicationCore.Pocos;
using Infrastructure.MySQL;
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
    internal static Mock<MySQLDbContext> GetMockForTestingLoadController()
    {
        var options = new DbContextOptionsBuilder<MySQLDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;

        var mock = new Mock<MySQLDbContext>(options);

        var genres = LoadDataSets.GetUnorderedGenres().AsQueryable().BuildMockDbSet();
        var publishers = LoadDataSets.GetUnorderedPublishers().AsQueryable().BuildMockDbSet();
        var formats = LoadDataSets.GetUnorderedFormats().AsQueryable().BuildMockDbSet();

        mock.Setup<DbSet<Genre>>(m => m.Genres).Returns(genres.Object);
        mock.Setup<DbSet<Publisher>>(m => m.Publishers).Returns(publishers.Object);
        mock.Setup<DbSet<Format>>(m => m.Formats).Returns(formats.Object);
        mock.Setup(m => m.IsDatabaseAvailable()).Returns(() => true);

        return mock;
    }
}