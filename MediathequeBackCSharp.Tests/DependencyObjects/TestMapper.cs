using ApplicationCore.AutoMapper;
using AutoMapper;

namespace MediathequeBackCSharp.Tests.DependencyObjects;

internal static class TestMapper
{
    internal static Mapper GetMapper()
    {
        var mappingProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
        return new Mapper(configuration);
    }
}