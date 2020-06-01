using Catalog.API.Dto;
using Catalog.API.Infrastructure.Respositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogBooksRepository _catalogBooksRepository;
        public CatalogController(ICatalogBooksRepository catalogBooksRepository)
        {
            _catalogBooksRepository = catalogBooksRepository;
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedItems<Domain.Model.Book>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ItemsAsync(
            [FromQuery]int pageSize = 10,
            [FromQuery]int pageIndex = 1,
            [FromQuery]string searchTerm = null)
        {
            if (pageSize <= 0)
                pageSize = 10;

            if (pageSize > 50)
                pageSize = 50;

            if (pageIndex <= 0)
                pageIndex = 1;

            var totalItems = await _catalogBooksRepository.GetTotalCountAsync(searchTerm);

            var itemsOnPage = await _catalogBooksRepository.GetPageAsync(
                pageSize, pageIndex, searchTerm);

            var model = new PaginatedItems<Domain.Model.Book>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet]
        [Route("items/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Domain.Model.Book), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Domain.Model.Book>> ItemByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var item = await _catalogBooksRepository.GetAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }
    }
}