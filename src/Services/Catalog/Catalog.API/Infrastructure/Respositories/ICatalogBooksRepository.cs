using Catalog.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.Respositories
{
    public interface ICatalogBooksRepository
    {
        Task<Book> GetAsync(string id);
        Task<IEnumerable<Book>> GetPageAsync(int pageSize, int pageIndex, string searchTerm = null);
        Task<long> GetTotalCountAsync(string searchTerm = null);
    }
}