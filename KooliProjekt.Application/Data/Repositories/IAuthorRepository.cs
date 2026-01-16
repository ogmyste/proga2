using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    /// <summary>
    /// Autorite repository interface
    /// </summary>
    public interface IAuthorRepository
    {
        Task<Author> GetByIdAsync(int id);
        Task SaveAsync(Author entity);
        Task DeleteAsync(Author entity);
    }
}
