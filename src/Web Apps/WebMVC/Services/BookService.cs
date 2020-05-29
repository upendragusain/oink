using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookService> _logger;

        public BookService(
            HttpClient httpClient,
            ILogger<BookService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<BookViewModel> GetItems(
            int pageSize, int pageIndex)
        {
            var uri = "https://localhost:44392/api/catalog/items?pageSize=" 
                + pageSize + "&pageIndex=" + pageIndex;

            var responseString = await _httpClient.GetStringAsync(uri);

            var pageBooks = JsonConvert.DeserializeObject<BookViewModel>(responseString);

            return pageBooks;
        }


        public async Task<BookDetailViewModel> GetItem(string id)
        {
            var uri = "https://localhost:44392/api/catalog/items/" + id;

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var book = JsonConvert.DeserializeObject<BookDetailViewModel>(
                    await response.Content.ReadAsStringAsync());

                return book;
            }

            return null;
        }
    }
}
