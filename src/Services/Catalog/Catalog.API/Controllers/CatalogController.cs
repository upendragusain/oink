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

            var books = itemsOnPage.Select(_ => MapToBook(_));

            var model = new PaginatedItemsViewModel<BookViewModel>(
                pageIndex, pageSize, totalItems, books);

            return Ok(model);
        }

        private BookViewModel MapToBook(CatalogItem catalogItem)
        {
            var book = (Book)catalogItem;
            return new BookViewModel()
            {
                AuthorName = book.AuthorName,
                Id = book.Id,
                Name = book.Name,
                Price = book.Price,
                Description = book.Description,
                Publisher = book.Publisher
            };
        }

        //[HttpGet("{id}")]
        //[Route("Stream")]
        //public async Task<IActionResult> DownloadImage()
        //{
        //    Stream stream = _context.MarketingData.
        //    string mimeType = "image/jpeg";
        //    return new FileStreamResult(stream, mimeType)
        //    {
        //        FileDownloadName = "image.jpeg"
        //    };
        //}
    }
}