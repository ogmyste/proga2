using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    /// <summary>
    /// Raamatute repository klass
    /// </summary>
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext dbContext) : 
            base(dbContext)
        {
        }

        /// <summary>
        /// Tagastab raamatu koos autoriga
        /// </summary>
        public override async Task<Book> GetByIdAsync(int id)
        {
            return await DbContext
                .Books
                .Include(book => book.Author)
                .Where(book => book.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
