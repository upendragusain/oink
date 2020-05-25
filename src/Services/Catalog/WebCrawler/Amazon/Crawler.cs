using HtmlAgilityPack;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Model;

namespace WebCrawler.Amazon
{
    public class Crawler : ICrawler<AmazonBook>
    {
        private readonly HttpClient _httpClient;

        private const string TITLE_AUTHORS_XPATH = 
            @"//*[contains(concat(' ', normalize-space(@class), ' '), ' a-spacing-none ') and contains(concat(' ', normalize-space(@class), ' '), ' a-section ')]";

        private const string TITLE_IMAGE_XPATH =
            @"//*[contains(concat(' ', normalize-space(@class), ' '), ' s-image ')]";

        private const string PAGE_ONE_PATH = 
            @"//*[contains(concat(' ', normalize-space(@class), ' '), ' a-pagination ')]";

        private const string PAGE_PATTERN =
            "&i=stripbooks&page=PAGE_NUMBER&qid=1590338955&ref=sr_pg_PAGE_NUMBER";

        ConcurrentBag<AmazonBook> _concurrentBooksCollection = new ConcurrentBag<AmazonBook>();

        public Crawler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AmazonBook>> ProcessAsync()
        {
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
            var content = await response.Content.ReadAsStreamAsync();

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(content);

            var firstPage = GetPageLink(htmlDocument);
            if (string.IsNullOrWhiteSpace(firstPage))
            {
                return null;
            }

            Task[] taskArray = new Task[3];

            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew(() => ProcessPage(i));
            }
            Task.WaitAll(taskArray);

            return _concurrentBooksCollection;
        }

        public async void ProcessPage(int pageNumber)
        {
            var pageLink = "";
            if (pageNumber > 1)
            {
                pageLink = PAGE_PATTERN.Replace("PAGE_NUMBER",
                    pageNumber.ToString(), StringComparison.OrdinalIgnoreCase);
            }

            var uri = new Uri(_httpClient.BaseAddress + pageLink);
            var response = await _httpClient.GetAsync(uri);
            var content = await response.Content.ReadAsStreamAsync();

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(content);

            var titleAuthors = GetTitleAuthor(htmlDocument, TITLE_AUTHORS_XPATH);

            var titleImageUris = GetTitleImageUri(htmlDocument, TITLE_IMAGE_XPATH);

            MapToAmazonBook(titleAuthors, titleImageUris);
        }

        private void MapToAmazonBook(
            Dictionary<string, string> titleAuthors, 
            Dictionary<string, string> titleImageUris)
        {
            foreach (var item in titleAuthors)
            {
                _concurrentBooksCollection.Add(new AmazonBook()
                {
                    Title = item.Key,
                    Author = item.Value,
                    Uri = titleImageUris[item.Key]
                });
            }
        }

        private string GetPageLink(HtmlDocument htmlDoc)
        {
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes(PAGE_ONE_PATH))
            {
                var node_li = link.Descendants("li").ToList();
                if (node_li[1] != null)
                {
                    var node_li_a = node_li[1].Descendants("a").FirstOrDefault();
                    if (node_li_a != null)
                    {
                        var node_li_a_img = node_li_a.Attributes["href"].Value;
                        return node_li_a_img.Trim();
                    }
                }
            }

            return null;
        }

        private Dictionary<string, string> GetTitleAuthor(HtmlDocument htmlDocument, string xPath)
        {
            var authorsTitle = new Dictionary<string, string>();

            foreach (HtmlNode htmlNode in htmlDocument.DocumentNode.SelectNodes(xPath))
            {
                var node_h2 = htmlNode.Descendants("h2").FirstOrDefault();
                if (node_h2 != null)
                {
                    var node_h2_a = node_h2.Descendants("a").FirstOrDefault();
                    if (node_h2_a != null)
                    {
                        var node_h2_a_span = node_h2_a.Descendants("span");
                        if (node_h2_a != null)
                        {
                            var authorDiv = htmlNode.Descendants("div").FirstOrDefault();
                            if (authorDiv != null)
                            {
                                var author_div_a = authorDiv.Descendants("a").FirstOrDefault();
                                if (author_div_a != null)
                                {
                                    authorsTitle.Add(node_h2_a.InnerText.Trim(), author_div_a.InnerText.Trim());
                                }
                            }
                        }
                    }
                }
            }

            return authorsTitle;
        }

        private Dictionary<string, string> GetTitleImageUri(HtmlDocument htmlDocument, string xPath)
        {
            var titleImageUris = new Dictionary<string, string>();

            foreach (HtmlNode htmlNode in htmlDocument.DocumentNode.SelectNodes(xPath))
            {
                var node_src = htmlNode.Attributes["src"].Value;
                var node_alt = htmlNode.Attributes["alt"].Value;
                titleImageUris.Add(node_alt.Trim(), node_src.Trim());
            }

            return titleImageUris;
        }
    }
}
