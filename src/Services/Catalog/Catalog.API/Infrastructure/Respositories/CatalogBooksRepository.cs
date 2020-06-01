using Catalog.API.Infrastructure.DataContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.Respositories
{
    public class CatalogBooksRepository : ICatalogBooksRepository
    {
        private readonly CatalogBooksReadDataContext _context;

        public CatalogBooksRepository(CatalogBooksReadDataContext context)
        {
            _context = context;
        }

        public async Task<Domain.Model.Book> GetAsync(string id)
        {
            var bookDto
                =  await _context.GetSingleOrDefaultAsync(id);

            return MapToBook(bookDto);
        }

        public async Task<long> GetTotalCountAsync(string searchTerm = null)
        {
            return await _context.GetTotalCountAsync(searchTerm);
        }

        public async Task<IEnumerable<Domain.Model.Book>> GetPageAsync(
            int pageSize, int pageIndex, string searchTerm = null)
        {
            var itemsOnPage = await _context.GetPageDocumentsAsync(
                pageSize, pageIndex, searchTerm);

            var books = itemsOnPage.Select(_ => MapToBook(_));

            return books;
        }

        private Domain.Model.Book MapToBook(Dto.Book dto)
        {
            return new Domain.Model.Book()
            {
                AuthorName = dto.AuthorName,
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Publisher = dto.Publisher,
                Media = dto.Media,
                Reviews = dto.Reviews
            };
        }
    }
}
