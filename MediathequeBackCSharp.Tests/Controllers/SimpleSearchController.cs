using ApplicationCore.DTOs.SearchDTOs;
using Infrastructure.MySQL.Repositories;
using MediathequeBackCSharp.Managers.SearchManagers;
using MediathequeBackCSharp.Services.Aggregates;
using MediathequeBackCSharp.Tests.DependencyObjects;
using MediathequeBackCSharp.Tests.Mocks;
using MediathequeBackCSharp.Texts;
using Microsoft.Extensions.Logging;

namespace MediathequeBackCSharp.Tests.Controllers;

[TestClass]
public class SimpleSearchController
{
    [TestMethod]
    public async Task PostWithMySQLDatabase()
    {
        var dbContextMock = MySQLDbContextMock.GetMockForTestingSearchController();
        var repository = new MySQLSimpleSearchRepository(dbContextMock.Object);
        var logger = new Logger<MediathequeBackCSharp.Controllers.SearchControllers.SimpleSearchController>(LoggerFactory.Create(builder => { }));
        var mapper = TestMapper.GetMapper();
        var textsManager = TextsManager.Instance;
        var sqlSearchService = new Services.MySQLSimpleSearchService(mapper, textsManager, repository);
        var allSimpleServices = new AllSimpleSearchServices(sqlSearchService, null!);
        var searchManager = new SimpleSearchManager(allSimpleServices, mapper, textsManager);
        var simpleSearchController = new MediathequeBackCSharp.Controllers.SearchControllers.SimpleSearchController(logger, searchManager);

        var searchCriteria = new SimpleSearchDTO
        {
            SimpleCriterion = "doumeizel"
        };

        var results = await searchManager.SearchForResults(searchCriteria, ApplicationCore.Enums.SearchTypeEnum.MySQLSimple);

        Assert.IsNotNull(results);
        Assert.IsTrue(results.Count == 2);
    }
}