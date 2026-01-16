using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Books
{
    /// <summary>
    /// Kasutab IBookRepositoryt
    /// </summary>
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, OperationResult<object>>
    {
        private readonly IBookRepository _bookRepository;

        public GetBookQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult<object>> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var book = await _bookRepository.GetByIdAsync(request.Id);

            if (book == null)
            {
                return result;
            }

            result.Value = new
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Author = new
                {
                    book.Author.Id,
                    book.Author.FirstName,
                    book.Author.LastName
                }
            };

            return result;
        }
    }
}
