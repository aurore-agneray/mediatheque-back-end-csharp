using ApplicationCore.DTOs.SearchDTOs;

namespace ApplicationCore.Tests.TestData;

internal static class SearchServiceTestsData
{
    internal static IEnumerable<EditionResultDTO> Editions = [

        new EditionResultDTO {
            Id = 1,
            Volume = "51",
            Series = new SeriesResultDTO {
                SeriesName = "Narnia"
            }
        },
        new EditionResultDTO {
            Id = 2,
            Volume = "10",
            Series = null
        },
        new EditionResultDTO {
            Id = 3,
            Volume = "46",
            Series = new SeriesResultDTO {
                SeriesName = "Narnia"
            }
        },
        new EditionResultDTO {
            Id = 4,
            Volume = "7",
            Series = null
        },
        new EditionResultDTO {
            Id = 5,
            Volume = "11",
            Series = new SeriesResultDTO {
                SeriesName = "Narnia"
            }
        },
        new EditionResultDTO {
            Id = 6,
            Volume = "1",
            Series = null
        },
        new EditionResultDTO {
            Id = 7,
            Volume = "9",
            Series = null
        },
        new EditionResultDTO {
            Id = 8,
            Volume = "100",
            Series = null
        },
        new EditionResultDTO {
            Id = 9,
            Volume = "37",
            Series = new SeriesResultDTO {
                SeriesName = "Pokemon"
            }
        },
        new EditionResultDTO {
            Id = 10,
            Volume = "16",
            Series = null
        },
        new EditionResultDTO {
            Id = 11,
            Volume = "86",
            Series = new SeriesResultDTO {
                SeriesName = "Pokemon"
            }
        },
    ];
}