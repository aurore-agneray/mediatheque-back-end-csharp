using ApplicationCore.Pocos;
using Infrastructure.MySQL;
using Infrastructure.MySQL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace MediathequeBackCSharp.Tests.Mocks;

/// <summary>
/// Used for mimicking the behaviour of the MySQLLoadRepository
/// </summary>
internal static class MySQLLoadRepositoryMock
{
    internal static Mock<MySQLLoadRepository> GetMock()
    {
        var options = new DbContextOptionsBuilder<MySQLDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;

        var dbContext = new MySQLDbContext(options);

        var mock = new Mock<MySQLLoadRepository>(
            () => new MySQLLoadRepository(dbContext)
        );

        mock.Setup(m => m.GetGenres()).Returns(() => Queryable.AsQueryable(new List<Genre> {
            new() { Id = 5, Name = "Polar" },
            new() { Id = 2, Name = "Roman" },
            new() { Id = 3, Name = "Documentaire" },
            new() { Id = 1, Name = "Historique" }
        }));

        mock.Setup(m => m.GetPublishers()).Returns(() => Queryable.AsQueryable(new List<Publisher> {
            new() { Id = 2, PublishingHouse = "France Loisirs" },
            new() { Id = 1, PublishingHouse = "Le Dilettante" },
            new() { Id = 5, PublishingHouse = "Hachette" },
            new() { Id = 3, PublishingHouse = "J'ai Lu" }
        }));

        mock.Setup(m => m.GetFormats()).Returns(() => Queryable.AsQueryable(new List<Format> {
            new() { Id = 4, Name = "Imprimé 18 cm" },
            new() { Id = 1, Name = "Livre de poche" },
            new() { Id = 3, Name = "Magazine" },
            new() { Id = 5, Name = "Imprimé 24 cm" }
        }));

        return mock;
    }
}