using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Books
{
    [ExcludeFromCodeCoverage]
    public class ListBooksQuery : IRequest<OperationResult<PagedResult<Book>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string Title { get; set; }
        public int? AuthorId { get; set; }
    }
}
