using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Authors
{
    public class GetAuthorQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
