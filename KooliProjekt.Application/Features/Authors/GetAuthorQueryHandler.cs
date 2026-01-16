using System.Linq;
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
    public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, OperationResult<object>>
    {
        private readonly IAuthorRepository _authorRepository;

        public GetAuthorQueryHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<object>> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var author = await _authorRepository.GetByIdAsync(request.Id);

            if (author == null)
            {
                return result;
            }

            result.Value = new
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Books = author.Books?.Select(book => new
                {
                    book.Id,
                    book.Title,
                    book.Year
                })
            };

            return result;
        }
    }
}
