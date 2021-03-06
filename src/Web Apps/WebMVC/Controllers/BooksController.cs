﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private IBookService _bookService;

        public BooksController(ILogger<BooksController> logger,
            IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        public async Task<IActionResult> Index(
            int? page, string searchTerm = null)
        {

            if (page <= 0)
                page = 1;

            var itemsPage = 10;

            var pageBooks = await _bookService.GetItems(
                itemsPage, page ?? 1, searchTerm);

            var vm = new IndexViewModel()

            {
                Books = pageBooks.Data,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page ?? 1,
                    ItemsPerPage = pageBooks.Data.Count,
                    TotalItems = pageBooks.Count,
                    TotalPages = (int)Math.Ceiling((decimal)pageBooks.Count / itemsPage),
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