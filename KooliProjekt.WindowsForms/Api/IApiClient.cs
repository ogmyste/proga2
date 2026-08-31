using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<Book>>> List(int page, int pageSize);
        Task<OperationResult> Save(Book book);
        Task<OperationResult> Delete(int id);
    }
}
