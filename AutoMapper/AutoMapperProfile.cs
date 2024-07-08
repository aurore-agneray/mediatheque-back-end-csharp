using AutoMapper;
using mediatheque_back_csharp.Dtos;
using mediatheque_back_csharp.Pocos;

namespace mediatheque_back_csharp.AutoMapper;

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
        CreateMap<Format, FormatDTO>();
        CreateMap<Genre, GenreDTO>();
        CreateMap<Publisher, PublisherDTO>();
        CreateMap<Series, SeriesDTO>();
    }
}