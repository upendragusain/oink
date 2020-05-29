using Catalog.API.Infrastructure;
using Catalog.API.Model;
using Catalog.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly BookReadDataContext _context;
        public CatalogController(BookReadDataContext context)
        {
            _context = context;
        }

        // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items")]
        public async Task<IActionResult> ItemsAsync(
            [FromQuery]int pageSize = 10, 
            [FromQuery]int pageIndex = 0)
        {
            var totalItems = await _context.GetAllDocumentsCountAsync();

            var itemsOnPage = await _context.GetDocumentsForAPage(pageSize, pageIndex);

            var books = itemsOnPage.Select(_ => MapToBookViewModel(_));

            var model = new PaginatedItemsViewModel<BookViewModel>(
                pageIndex, pageSize, totalItems, books);

            return Ok(model);
        }

        [HttpGet]
        [Route("items/{id}")]
        public async Task<ActionResult<BookViewModel>> ItemByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var item = await _context.GetSingleOrDefaultAsync(id);

            return item != null ? MapToBookViewModel(item) 
                : (ActionResult<BookViewModel>) NotFound();
        }

        private BookViewModel MapToBookViewModel(CatalogItem catalogItem)
        {
            var book = (Book)catalogItem;
            return new BookViewModel()
            {
                AuthorName = book.AuthorName,
                Id = book.Id,
                Name = book.Name,
                Price = book.Price,
                Description = book.Description,
                Publisher = book.Publisher,
                ImageUrl = book.Images?.FirstOrDefault()?.Url,
                ImageContent = book.Images?.FirstOrDefault()?.Content
            };
        }
    }
}