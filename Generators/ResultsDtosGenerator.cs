using mediatheque_back_csharp.Constants;
using mediatheque_back_csharp.DTOs.SearchDTOs;
using mediatheque_back_csharp.Extensions;

namespace mediatheque_back_csharp.Generators;

/// <summary>
/// Describes methods which generates DTOs from other objects
/// </summary>
public static class ResultsDtosGenerator {
    
    /// <summary>
    /// Completes an EditionResultDTO with the data contained into a dictionary
    /// </summary>
    /// <param name="editionData">A dictionary whose key is the name of a property 
    /// (like "isbn", "subtitle", etc) and the value is ... the value (lol) of this property</param>
    /// <param name="bookId">ID of the concerned book</param>
    public static EditionResultDTO GenerateEditionResultDTO(Dictionary<string, string> editionData, int bookId)
    {
        if (editionData == null) {
            return new EditionResultDTO();
        }

        return new EditionResultDTO {
            BookId = bookId,
            Isbn = editionData.GetDatumIfValid(BnfPropertiesConsts.ISBN),
            Subtitle = editionData.GetDatumIfValid(BnfPropertiesConsts.SUBTITLE),
            PublicationDateBnf = editionData.GetDatumIfValid(BnfPropertiesConsts.PUBLICATION_DATE_BNF),
            Volume = editionData.GetDatumIfValid(BnfPropertiesConsts.VOLUME),
            Summary = editionData.GetDatumIfValid(BnfPropertiesConsts.SUMMARY),
            Series = new SeriesResultDTO {
                SeriesName = editionData.GetDatumIfValid(BnfPropertiesConsts.SERIES_NAME)
            },
            Publisher = new PublisherResultDTO {
                PublishingHouse = editionData.GetDatumIfValid(BnfPropertiesConsts.PUBLISHER)
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
        if (string.IsNullOrEmpty(titleAndAuthorConcatenation)) {
            return new BookResultDTO();
        }

        // Extracts the book's title and the author's name
        var titleAndAuthorArray = titleAndAuthorConcatenation.Split(BnfConsts.TITLE_AND_AUTHOR_NAME_SEPARATOR);

        if (titleAndAuthorArray.Count() < 2) {
            return new BookResultDTO();
        }

        return new BookResultDTO {
            Title = titleAndAuthorArray[0],
            Author = new AuthorResultDTO {
                CompleteName = titleAndAuthorArray[1]
            },
            Id = bookId
        };
    }
}