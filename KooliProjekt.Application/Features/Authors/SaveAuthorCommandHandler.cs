using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Authors
{
    public class SaveAuthorCommandHandler : IRequestHandler<SaveAuthorCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAuthorCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id < 0)
            {
                result.AddError("Request ID cannot be negative");
                return result;
            }

            var author = new Author();
            if (request.Id == 0)
            {
                await _dbContext.Authors.AddAsync(author);
            }
            else
            {
                author = await _dbContext.Authors.FindAsync(request.Id);
                if (author == null)
                {
                    result.AddError("Cannot find author with ID " + request.Id);
                    return result;
                }
            }

            author.FirstName = request.FirstName;
            author.LastName = request.LastName;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
