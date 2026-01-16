using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Books
{
    /// <summary>
    /// Raamatu kustutamise commandi handler.
    /// Handle meetodis toimub tegelik kustutamine.
    /// </summary>
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteBookCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .Books
                .Where(b => b.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
