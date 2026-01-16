using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    /// <summary>
    /// Raamatute repository interface
    /// </summary>
    public interface IBookRepository
    {
        Task<Book> GetByIdAsync(int id);
        Task SaveAsync(Book entity);
        Task DeleteAsync(Book entity);
    }
}
