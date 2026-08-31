using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Books
{
    public class ListBooksQueryHandler : IRequestHandler<ListBooksQuery, OperationResult<PagedResult<Book>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListBooksQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Book>>> Handle(ListBooksQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<Book>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(book => book.Title.Contains(request.Title));
            }

            if (request.AuthorId.HasValue)
            {
                query = query.Where(book => book.AuthorId == request.AuthorId.Value);
            }

            result.Value = await query
                .OrderBy(book => book.Title)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
