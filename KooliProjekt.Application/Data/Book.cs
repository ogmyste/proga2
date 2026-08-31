using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Data
{
[ExcludeFromCodeCoverage]
    public class Book : Entity
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Range(1000, 2100)]
        public int Year { get; set; }

        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }
}
