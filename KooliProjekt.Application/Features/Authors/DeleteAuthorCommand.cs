using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Authors
{
    /// <summary>
    /// Autori kustutamise command
    /// </summary>
    public class DeleteAuthorCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
