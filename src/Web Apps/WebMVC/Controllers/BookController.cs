using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private IBookService _bookService;

        public BookController(ILogger<BookController> logger,
            IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string bookId)
        {
            var book = await _bookService.GetItem(bookId);
            return View(book);
        }
    }
}