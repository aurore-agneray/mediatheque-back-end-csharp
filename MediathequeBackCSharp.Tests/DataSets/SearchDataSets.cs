using ApplicationCore.Pocos;

namespace MediathequeBackCSharp.Tests.DataSets;

/// <summary>
/// Data set that can be used for testing all the SearchControllers
/// </summary>
internal static class SearchDataSets
{
    public static Author[] Authors { get; } = [
        new() { Id = 1, Code = "JPD", FirstName = "Jean-Pierre", LastName = "Dupont" },
        new() { Id = 2, Code = "VD", FirstName = "Vincent", LastName = "Doumeizel" },
        new() { Id = 3, Code = "HI", FirstName = "Hajime", LastName = "Isayama" },
        new() { Id = 4, Code = "AC", FirstName = "Agatha", LastName = "Christie" }
    ];

    public static Genre[] Genres { get; } = [
        new() { Id = 1, Name = "Historique" },
        new() { Id = 2, Name = "Manga" },
        new() { Id = 3, Name = "Documentaire" },
        new() { Id = 4, Name = "Polar" }
    ];

    public static Publisher[] Publishers { get; } = [
        new() { Id = 1, Code = "LD", PublishingHouse = "Le Dilettante" },
        new() { Id = 2, Code = "FL", PublishingHouse = "France Loisirs" },
        new() { Id = 3, Code = "JL", PublishingHouse = "J'ai Lu" },
        new() { Id = 4, Code = "HA", PublishingHouse = "Hachette" }
    ];

    public static Format[] Formats { get; } = [
        new() { Id = 1, Name = "Livre de poche" },
        new() { Id = 2, Name = "Imprimé 24 cm" },
        new() { Id = 3, Name = "Magazine" },
        new() { Id = 4, Name = "Imprimé 18 cm" }
    ];

    public static Series[] Series { get; } = [
        new() { Id = 1, Code = "S1", Name = "Série 1" },
        new() { Id = 2, Code = "S2", Name = "Série 2" },
    ];

    public static Book[] Books { get; } = [
        new() { Id = 1, BookCode = "RA", Title = "La Révolution des Algues", AuthorId = 2, Author = Authors[1], GenreId = 3, Genre = Genres[2] },
        new() { Id = 2, BookCode = "AT", Title = "L'Attaque des Titans", AuthorId = 3, Author = Authors[2], GenreId = 2, Genre = Genres[1] },
        new() { Id = 3, BookCode = "ML", Title = "Un meurtre a eu lieu", AuthorId = 4, Author = Authors[3], GenreId = 4, Genre = Genres[3] },
        new() { Id = 4, BookCode = "LG", Title = "La Guerre de 100 ans", AuthorId = 4, Author = Authors[3], GenreId = 1, Genre = Genres[0] },
        new() { Id = 5, BookCode = "MN", Title = "Mort sur le Nil", AuthorId = 4, Author = Authors[3], GenreId = 4, Genre = Genres[3] }
    ];

    public static Edition[] Editions { get; } = [
        new() { 
            Id = 1,
            BookId = 1, Book = Books[0], 
            FormatId = 4, Format = Formats[3], 
            Isbn = "ALGUESFORMAT18CM", 
            PublicationDate = new DateTime(2021, 07, 13), PublicationYear = 2021, 
            PublisherId = 3, Publisher = Publishers[2] 
        },
        new() {
            Id = 2,
            BookId = 1, Book = Books[0],
            FormatId = 2, Format = Formats[1],
            Isbn = "ALGUESFORMAT24CM",
            PublicationDate = new DateTime(2022, 09, 01), PublicationYear = 2022,
            PublisherId = 3, Publisher = Publishers[2]
        }
    ];
}