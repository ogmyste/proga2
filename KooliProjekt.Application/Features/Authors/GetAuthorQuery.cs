using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Authors
{
    [ExcludeFromCodeCoverage]
    public class GetAuthorQuery : IRequest<OperationResult<AuthorDetailsDto>>
    {
        public int Id { get; set; }
    }
}
