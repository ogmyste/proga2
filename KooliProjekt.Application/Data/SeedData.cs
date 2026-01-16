using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    /// <summary>
    /// Testandmete generaator
    /// 
    /// Testandmed genereeritakse ainult siis kui mõni oluline 
    /// tabel on tühi.
    /// </summary>
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;

        public SeedData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Genereerib andmed
        /// </summary>
        public void Generate()
        {
            if (_dbContext.Authors.Any())
            {
                return;
            }

            var authors = new[]
            {
                new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" },
                new Author { FirstName = "Friedebert", LastName = "Tuglas" },
                new Author { FirstName = "Jaan", LastName = "Kross" },
                new Author { FirstName = "Oskar", LastName = "Luts" },
                new Author { FirstName = "Eduard", LastName = "Vilde" },
                new Author { FirstName = "Andrus", LastName = "Kivirähk" },
                new Author { FirstName = "Karl", LastName = "Ristikivi" },
                new Author { FirstName = "August", LastName = "Gailit" },
                new Author { FirstName = "Eno", LastName = "Raud" },
                new Author { FirstName = "Juhan", LastName = "Liiv" }
            };

            foreach (var author in authors)
            {
                _dbContext.Authors.Add(author);
            }

            var books = new[]
            {
                new Book { Title = "Tõde ja õigus", Year = 1926, Author = authors[0] },
                new Book { Title = "Kõrboja peremees", Year = 1922, Author = authors[0] },
                new Book { Title = "Väike Illimar", Year = 1937, Author = authors[1] },
                new Book { Title = "Keisri hull", Year = 1978, Author = authors[2] },
                new Book { Title = "Wikmani poisid", Year = 1988, Author = authors[2] },
                new Book { Title = "Kevade", Year = 1912, Author = authors[3] },
                new Book { Title = "Suvi", Year = 1913, Author = authors[3] },
                new Book { Title = "Mäeküla piimamees", Year = 1916, Author = authors[4] },
                new Book { Title = "Rehepapp", Year = 2000, Author = authors[5] },
                new Book { Title = "Mees, kes teadis ussisõnu", Year = 2007, Author = authors[5] },
                new Book { Title = "Hingede öö", Year = 1953, Author = authors[6] },
                new Book { Title = "Toomas Nipernaadi", Year = 1928, Author = authors[7] },
                new Book { Title = "Naksitrallid", Year = 1972, Author = authors[8] },
                new Book { Title = "Sipsik", Year = 1962, Author = authors[8] }
            };

            foreach (var book in books)
            {
                _dbContext.Books.Add(book);
            }

            _dbContext.SaveChanges();
        }
    }
}
