using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Authors
{
    /// <summary>
    /// Autori kustutamise command
    /// </summary>
[ExcludeFromCodeCoverage]
    public class DeleteAuthorCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
