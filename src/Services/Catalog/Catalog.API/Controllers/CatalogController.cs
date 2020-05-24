using System.Threading.Tasks;
using Catalog.API.Model;
using Catalog.API.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        //[HttpGet]
        //public async Task<ActionResult> ItemsAsync([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0, string ids = null)
        //{
        //    int totalItems = 100;
        //    int itemsOnPage = 10;

        //    var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

        //    return Ok(model);
        //}
    }
}