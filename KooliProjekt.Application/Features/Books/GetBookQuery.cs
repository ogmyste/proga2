using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Books
{
    [ExcludeFromCodeCoverage]
    public class GetBookQuery : IRequest<OperationResult<BookDetailsDto>>
    {
        public int Id { get; set; }
    }
}
