using ApplicationCore.DTOs.SearchDTOs;
using ApplicationCore.Extensions;
using Infrastructure.BnfApi;
using Infrastructure.BnfApi.Constants;

namespace MediathequeBackCSharp.Generators;

/// <summary>
/// Describes methods which generates Results DTOs from other objects for the BnF API search
/// </summary>
public static class BnfResultsDtosGenerator
{
    /// <summary>
    /// Completes an EditionResultDTO with the data contained into a dictionary
    /// </summary>
    /// <param name="editionData">A dictionary whose key is the name of a property 
    /// (like "isbn", "subtitle", etc) and the value is ... the value (lol) of this property</param>
    /// <param name="bookId">ID of the concerned book</param>
    public static EditionResultDTO GenerateEditionResultDTO(Dictionary<string, string> editionData, int bookId)
    {
        if (editionData == null)
        {
            return new EditionResultDTO();
        }

        return new EditionResultDTO
        {
            BookId = bookId,
            Isbn = editionData.GetValueOrEmptyString(BnfPropertiesConsts.ISBN),
            Subtitle = editionData.GetValueOrEmptyString(BnfPropertiesConsts.SUBTITLE),
            PublicationDateBnf = editionData.GetValueOrEmptyString(BnfPropertiesConsts.PUBLICATION_DATE_BNF),
            Volume = editionData.GetValueOrEmptyString(BnfPropertiesConsts.VOLUME),
            Summary = editionData.GetValueOrEmptyString(BnfPropertiesConsts.SUMMARY),
            Series = new SeriesResultDTO
            {
                SeriesName = editionData.GetValueOrEmptyString(BnfPropertiesConsts.SERIES_NAME)
            },
            Publisher = new PublisherResultDTO
            {
                PublishingHouse = editionData.GetValueOrEmptyString(BnfPropertiesConsts.PUBLISHER)
            }
        };
    }

    /// <summary>
    /// Completes a BookResultDTO with the given data
    /// </summary>
    /// <param name="titleAndAuthorConcatenation">Concatenation of the title and the author's name</param>
    /// <param name="bookId">Book ID</param>
    public static BookResultDTO GenerateBookResultDTO(string titleAndAuthorConcatenation, int bookId)
    {
        if (string.IsNullOrEmpty(titleAndAuthorConcatenation))
        {
            return new BookResultDTO();
        }

        // Extracts the book's title and the author's name
        var titleAndAuthorArray = titleAndAuthorConcatenation.Split(BnfGlobalConsts.TITLE_AND_AUTHOR_NAME_SEPARATOR);

        if (titleAndAuthorArray.Length < 2)
        {
            return new BookResultDTO();
        }

        return new BookResultDTO
        {
            Title = titleAndAuthorArray[0],
            Author = new AuthorResultDTO
            {
                CompleteName = titleAndAuthorArray[1]
            },
            Id = bookId
        };
    }
}