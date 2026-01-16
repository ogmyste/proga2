using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Books
{
    public class ListBooksQueryHandler : IRequestHandler<ListBooksQuery, OperationResult<PagedResult<Book>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListBooksQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Book>>> Handle(ListBooksQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Book>>();

            result.Value = await _dbContext
                .Books
                .Include(book => book.Author)
                .OrderBy(book => book.Title)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
