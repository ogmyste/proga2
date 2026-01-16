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

namespace KooliProjekt.Application.Features.Authors
{
    /// <summary>
    /// Autori kustutamise commandi handler.
    /// Handle meetodis toimub tegelik kustutamine.
    /// </summary>
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAuthorCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .Authors
                .Where(a => a.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
