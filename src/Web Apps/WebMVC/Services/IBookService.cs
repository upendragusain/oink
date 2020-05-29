using System.Collections.Generic;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IBookService
    {
        Task<BookViewModel> GetItems(
            int pageSize, int pageIndex, string searchTerm = null);

        Task<BookDetailViewModel> GetItem(string id);
    }
}