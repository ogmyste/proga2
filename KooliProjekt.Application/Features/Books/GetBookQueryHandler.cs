using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Books
{
    // 16.01.2026 - BookDetailsDto
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, OperationResult<BookDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetBookQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<BookDetailsDto>> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<BookDetailsDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .Books
                .Include(book => book.Author)
                .Where(book => book.Id == request.Id)
                .Select(book => new BookDetailsDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Year = book.Year,
                    AuthorId = book.AuthorId,
                    AuthorFirstName = book.Author.FirstName,
                    AuthorLastName = book.Author.LastName
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
