using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Catalog.API.Infrastructure;
using Catalog.API.Model;
using Catalog.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogReadDataContext _context;
        public CatalogController(CatalogReadDataContext context)
        {
            _context = context;
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