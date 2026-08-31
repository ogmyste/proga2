using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Authors
{
    // 16.01.2026 - AuthorDetailsDto
    public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, OperationResult<AuthorDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAuthorQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<AuthorDetailsDto>> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<AuthorDetailsDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .Authors
                .Include(author => author.Books)
                .Where(author => author.Id == request.Id)
                .Select(author => new AuthorDetailsDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Books = author.Books.Select(book => new BookDto
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Year = book.Year
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
