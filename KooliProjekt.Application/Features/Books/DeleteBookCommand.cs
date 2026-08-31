using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Books
{
    /// <summary>
    /// Raamatu kustutamise command
    /// </summary>
[ExcludeFromCodeCoverage]
    public class DeleteBookCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
