using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Authors
{
    public class ListAuthorsQuery : IRequest<OperationResult<PagedResult<Author>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
