using Catalog.API.Model;
using HtmlAgilityPack;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WebCrawler.Amazon
{
    public class Crawler : ICrawler<CatalogItem>
    {
        private const string XPATH = @"//div[@data-asin]";//data-asin="0702300179"

        public async Task<IEnumerable<CatalogItem>> ProcessAsync(string pageUrl)
        {
            var pageBooks = new List<CatalogItem>();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(pageUrl);
            if (!response.IsSuccessStatusCode)
                return pageBooks;

            var content = await response.Content.ReadAsStreamAsync();

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(content);

            HtmlNodeCollection htmlNodeCollection = null;
            htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes(XPATH);

            if (htmlNodeCollection == null || !htmlNodeCollection.Any())
                throw new XPathNotFoundException($"XPath '{XPATH}' for the uri '{pageUrl}' was not found");

            foreach (HtmlNode htmlNode in htmlNodeCollection)
            {
                var node_img = htmlNode.Descendants("img").FirstOrDefault();
                if (node_img != null)
                {
                    var node_src = node_img.Attributes["src"].Value;
                    var node_alt = node_img.Attributes["alt"].Value;

                    var node_author = htmlNode.Descendants("a")
                        .Where(d => d.HasClass("a-size-base") && d.HasClass("a-link-normal"))
                        .FirstOrDefault();

                    if (node_author != null)
                    {
                        var bookName = HttpUtility.HtmlDecode(node_alt.Trim());
                        Log.Information("Found book - {0}", bookName);
                        CatalogItem book = new Book()
                        {
                            Name = bookName,
                            Price = GetBookPriceRandom(),
                            Department = new DepartmentType() { Name = "Books" },
                            Images = new List<CatalogImage>()
                            {
                                new CatalogImage(){ Url = node_src.Trim() }
                            },
                            AuthorName = HttpUtility.HtmlDecode(node_author.InnerText.Trim()),
                        };
                        pageBooks.Add(book);
                    }
                }
            }

            return pageBooks;
        }

        private decimal GetBookPriceRandom()
        {
            return Math.Round((decimal)(new Random().NextDouble() * new Random().Next(10, 100)), 2);
        }
    }
}
