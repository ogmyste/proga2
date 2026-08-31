using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Authors
{
    /// <summary>
    /// Autori kustutamise commandi handler.
    /// Handle meetodis toimub tegelik kustutamine.
    /// </summary>
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAuthorCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
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
            var author = await _dbContext
                .Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == request.Id);

            if (author == null)
            {
                return result;
            }

            _dbContext.Books.RemoveRange(author.Books);
            _dbContext.Authors.Remove(author);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
