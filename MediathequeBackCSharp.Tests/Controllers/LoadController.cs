using ApplicationCore.AutoMapper;
using ApplicationCore.Dtos;
using AutoMapper;
using MediathequeBackCSharp.Managers;
using MediathequeBackCSharp.Tests.Mocks;
using MediathequeBackCSharp.Texts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MediathequeBackCSharp.Tests.Controllers;

[TestClass]
public class LoadController
{
    private static Mapper GetMapper()
    {
        var mappingProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
        return new Mapper(configuration);
    }

    /// <summary>
    /// Should return a LoadDTO objects with 3 lists : genres, publishers and formats
    /// </summary>
    [TestMethod]
    public async Task Get()
    {
        var repositoryMock = MySQLLoadRepositoryMock.GetMock();
        var logger = new Logger<MediathequeBackCSharp.Controllers.LoadController>(LoggerFactory.Create(builder => { }));
        var mapper = GetMapper();
        var textsManager = TextsManager.Instance;
        var loadManager = new LoadManager(repositoryMock.Object, logger, mapper, textsManager);
        var loadController = new MediathequeBackCSharp.Controllers.LoadController(loadManager);
        var expectedDtos = new LoadDTO
        {
            Genres = [
                new() { Id = 5, Name = "Polar" },
                new() { Id = 2, Name = "Roman" },
                new() { Id = 3, Name = "Documentaire" },
                new() { Id = 1, Name = "Historique" }
            ],
            Publishers = [
                new() { Id = 2, Name = "France Loisirs" },
                new() { Id = 1, Name = "Le Dilettante" },
                new() { Id = 5, Name = "Hachette" },
                new() { Id = 3, Name = "J'ai Lu" }
            ],
            Formats = [
                new() { Id = 4, Name = "Imprimé 18 cm" },
                new() { Id = 1, Name = "Livre de poche" },
                new() { Id = 3, Name = "Magazine" },
                new() { Id = 5, Name = "Imprimé 24 cm" }
            ]
        };

        var result = await loadController.Get() as ObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsNotNull(result.Value);

        var lists = result.Value as LoadDTO;

        Assert.IsNotNull(lists);
        Assert.IsTrue(lists.Genres.Count > 0);
        Assert.IsTrue(lists.Publishers.Count > 0);
        Assert.IsTrue(lists.Formats.Count > 0);
        Assert.AreEqual(expectedDtos.Genres[2].Name, lists.Genres[0].Name); // Genre = "Documentaire"
        Assert.AreEqual(expectedDtos.Genres[1].Name, lists.Genres[3].Name); // Genre = "Roman"
        Assert.AreEqual(expectedDtos.Publishers[0].Name, lists.Publishers[0].Name); // Publisher = "France Loisirs"
        Assert.AreEqual(expectedDtos.Publishers[2].Name, lists.Publishers[1].Name); // Publisher = "Hachette"
        Assert.AreEqual(expectedDtos.Formats[0].Name, lists.Formats[0].Name); // Format = "Imprimé 18 cm"
        Assert.AreEqual(expectedDtos.Formats[1].Name, lists.Formats[2].Name); // Format = "Livre de poche"
    }
}