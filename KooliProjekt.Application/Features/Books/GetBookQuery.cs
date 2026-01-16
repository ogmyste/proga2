using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Books
{
    public class GetBookQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
