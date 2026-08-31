using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.BlazorWasm
{
    public interface IApiClient
    {
        Task<OperationResult<Book>> Get(int id);
        Task<OperationResult<PagedResult<Book>>> List(int page, int pageSize);
        Task<OperationResult> Save(Book book);
        Task<OperationResult> Delete(int id);
    }
}
