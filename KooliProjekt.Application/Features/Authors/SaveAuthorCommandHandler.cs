using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Authors
{
    /// <summary>
    /// Kasutab IAuthorRepositoryt
    /// </summary>
    public class SaveAuthorCommandHandler : IRequestHandler<SaveAuthorCommand, OperationResult>
    {
        private readonly IAuthorRepository _authorRepository;

        public SaveAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult> Handle(SaveAuthorCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var author = new Author();
            if (request.Id != 0)
            {
                author = await _authorRepository.GetByIdAsync(request.Id);
            }

            author.FirstName = request.FirstName;
            author.LastName = request.LastName;

            await _authorRepository.SaveAsync(author);

            return result;
        }
    }
}
