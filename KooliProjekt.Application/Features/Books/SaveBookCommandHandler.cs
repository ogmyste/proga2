using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Books
{
    public class SaveBookCommandHandler : IRequestHandler<SaveBookCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveBookCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveBookCommand request, CancellationToken cancellationToken)
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

            var book = new Book();
            if (request.Id == 0)
            {
                await _dbContext.Books.AddAsync(book);
            }
            else
            {
                book = await _dbContext.Books.FindAsync(request.Id);
                if (book == null)
                {
                    result.AddError("Cannot find book with ID " + request.Id);
                    return result;
                }
            }

            book.Title = request.Title;
            book.Year = request.Year;
            book.AuthorId = request.AuthorId;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
