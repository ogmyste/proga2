using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Books
{
    /// <summary>
    /// Raamatu kustutamise command
    /// </summary>
    public class DeleteBookCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
