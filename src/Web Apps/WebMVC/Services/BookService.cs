using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookService> _logger;
        private readonly IOptions<AppSettings> _settings;

        private readonly string _remoteServiceBaseUrl;

        public BookService(
            HttpClient httpClient,
            ILogger<BookService> logger,
            IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _logger = logger;
            _settings = settings;
            _remoteServiceBaseUrl = $"{_settings.Value.CatalogUrl}/api/catalog";
        }

        public async Task<BookViewModel> GetItems(
            int pageSize, int pageIndex, string searchTerm = null)
        {
            var uri = $"{_remoteServiceBaseUrl}" +
                $"/items?pageSize={pageSize}&pageIndex={pageIndex}&searchterm={searchTerm}";

            var responseString = await _httpClient.GetStringAsync(uri);

            var pageBooks = JsonConvert.DeserializeObject<BookViewModel>(responseString);

            return pageBooks;
        }


        public async Task<BookDetailViewModel> GetItem(string id)
        {
            var uri = $"{_remoteServiceBaseUrl}/items/{id}";

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
