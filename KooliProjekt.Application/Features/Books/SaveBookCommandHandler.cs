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
    public class SaveBookCommandHandler : IRequestHandler<SaveBookCommand, OperationResult>
    {
        private readonly IBookRepository _bookRepository;

        public SaveBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult> Handle(SaveBookCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var book = new Book();
            if (request.Id != 0)
            {
                book = await _bookRepository.GetByIdAsync(request.Id);
            }

            book.Title = request.Title;
            book.Year = request.Year;
            book.AuthorId = request.AuthorId;

            await _bookRepository.SaveAsync(book);

            return result;
        }
    }
}
