using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebMVC.Models;
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

        public async Task<IActionResult> Index(
            int? page, string searchTerm = null)
        {
            var itemsPage = 10;

            var pageBooks = await _bookService.GetItems(
                itemsPage, page ?? 0, searchTerm);

            var vm = new IndexViewModel()
            {
                Books = pageBooks.Data,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = pageBooks.Data.Count,
                    TotalItems = pageBooks.Count,
                    TotalPages = (int)Math.Ceiling(((decimal)pageBooks.Count / itemsPage)),
                    SearchTerm = searchTerm
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            //ViewBag.BasketInoperativeMsg = errorMsg;

            return View(vm);
        }

        public async Task<IActionResult> Detail(string bookId)
        {
            var book = await _bookService.GetItem(bookId);
            return View(book);
        }
    }
}