using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Books
{
    /// <summary>
    /// Raamatu kustutamise commandi handler.
    /// Handle meetodis toimub tegelik kustutamine.
    /// </summary>
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteBookCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            // InMemory ei toeta ExecuteDeleteAsync meetodit
            var book = await _dbContext
                .Books
                .FirstOrDefaultAsync(b => b.Id == request.Id);

            if (book == null)
            {
                return result;
            }

            _dbContext.Books.Remove(book);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
