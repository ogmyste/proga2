using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Authors
{
    public class ListAuthorsQueryHandler : IRequestHandler<ListAuthorsQuery, OperationResult<PagedResult<Author>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListAuthorsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Author>>> Handle(ListAuthorsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<Author>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.Authors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                query = query.Where(author => author.FirstName.Contains(request.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                query = query.Where(author => author.LastName.Contains(request.LastName));
            }

            result.Value = await query
                .OrderBy(author => author.LastName)
                .ThenBy(author => author.FirstName)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
