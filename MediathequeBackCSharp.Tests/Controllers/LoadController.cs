using ApplicationCore.AutoMapper;
using ApplicationCore.Dtos;
using AutoMapper;
using Infrastructure.MySQL.Repositories;
using MediathequeBackCSharp.Managers;
using MediathequeBackCSharp.Tests.DataSets;
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
        var dbContextMock = MySQLDbContextMock.GetMockForTestingLoadController();
        var repository = new MySQLLoadRepository(dbContextMock.Object);
        var logger = new Logger<MediathequeBackCSharp.Controllers.LoadController>(LoggerFactory.Create(builder => { }));
        var mapper = GetMapper();
        var textsManager = TextsManager.Instance;
        var loadManager = new LoadManager(repository, logger, mapper, textsManager);
        var loadController = new MediathequeBackCSharp.Controllers.LoadController(loadManager);
        var expectedDtos = LoadDataSets.GetFinalOrderedDataSet();

        var result = await loadController.Get() as ObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsNotNull(result.Value);

        var returnedDtos = result.Value as LoadDTO;

        Assert.IsNotNull(returnedDtos);
        Assert.IsTrue(returnedDtos.Genres.Count > 0);
        Assert.IsTrue(returnedDtos.Publishers.Count > 0);
        Assert.IsTrue(returnedDtos.Formats.Count > 0);

        for (int i = 0; i < expectedDtos.Genres.Count; i++)
        {
            Assert.AreEqual(expectedDtos.Genres[i].Name, returnedDtos.Genres[i].Name);
            Assert.AreEqual(expectedDtos.Genres[i].Id, returnedDtos.Genres[i].Id);
        }

        for (int i = 0; i < expectedDtos.Publishers.Count; i++)
        {
            Assert.AreEqual(expectedDtos.Publishers[i].Name, returnedDtos.Publishers[i].Name);
            Assert.AreEqual(expectedDtos.Publishers[i].Id, returnedDtos.Publishers[i].Id);
        }

        for (int i = 0; i < expectedDtos.Formats.Count; i++)
        {
            Assert.AreEqual(expectedDtos.Formats[i].Name, returnedDtos.Formats[i].Name);
            Assert.AreEqual(expectedDtos.Formats[i].Id, returnedDtos.Formats[i].Id);
        }
    }
}