using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    /// <summary>
    /// Autorite repository klass
    /// </summary>
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext dbContext) : 
            base(dbContext)
        {
        }

        /// <summary>
        /// Tagastab autori koos tema raamatutega
        /// </summary>
        public override async Task<Author> GetByIdAsync(int id)
        {
            return await DbContext
                .Authors
                .Include(author => author.Books)
                .Where(author => author.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
