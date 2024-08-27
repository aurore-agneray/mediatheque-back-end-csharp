using ApplicationCore.Dtos;
using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Pocos;
using AutoMapper;

namespace ApplicationCore.AutoMapper;

/// <summary>
/// Defines the profile used for configuring the AutoMapper package
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Main constructor of the class AutoMapperProfile
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<Author, AuthorDTO>();
        CreateMap<Book, BookDTO>();
        CreateMap<Edition, EditionDTO>();
        CreateMap<Publisher, PublisherDTO>();
        CreateMap<Series, SeriesDTO>();

        CreateMap<Format, NamedDTO>();
        CreateMap<Genre, NamedDTO>();
        CreateMap<Publisher, NamedDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PublishingHouse));

        CreateMap<Book, BookResultDTO>();
        CreateMap<Author, AuthorResultDTO>();
        CreateMap<Genre, GenreResultDTO>()
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Name));

        CreateMap<Edition, EditionResultDTO>();
        CreateMap<Format, FormatResultDTO>()
            .ForMember(dest => dest.FormatName, opt => opt.MapFrom(src => src.Name));
        CreateMap<Series, SeriesResultDTO>()
            .ForMember(dest => dest.SeriesName, opt => opt.MapFrom(src => src.Name));
        CreateMap<Publisher, PublisherResultDTO>();
    }
}