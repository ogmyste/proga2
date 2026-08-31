using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
    }
}
