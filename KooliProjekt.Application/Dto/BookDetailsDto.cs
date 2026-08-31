using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class BookDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int AuthorId { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
    }
}
