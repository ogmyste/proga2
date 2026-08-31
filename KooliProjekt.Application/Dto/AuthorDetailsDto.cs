using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class AuthorDetailsDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<BookDto> Books { get; set; } = new List<BookDto>();
    }
}
