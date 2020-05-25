﻿using HtmlAgilityPack;
using System;
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

            var titleAuthors = GetTitleAuthor(htmlDocument, TITLE_AUTHORS_XPATH);

            var titleImageUris = GetTitleImageUri(htmlDocument, TITLE_IMAGE_XPATH);

            return MapToAmazonBook(titleAuthors, titleImageUris);
        }

        private IEnumerable<AmazonBook> MapToAmazonBook(
            Dictionary<string, string> titleAuthors, 
            Dictionary<string, string> titleImageUris)
        {
            var books = new List<AmazonBook>();
            foreach (var item in titleAuthors)
            {
                books.Add(new AmazonBook()
                {
                    Title = item.Key,
                    Author = item.Value,
                    Uri = titleImageUris[item.Key]
                });
            }

            return books;
        }

        private void GetPageLink(HtmlDocument htmlDoc)
        {
            string x = @"//*[contains(concat(' ', normalize-space(@class), ' '), ' a-pagination ')]";
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes(x))
            {
                var node_li = link.Descendants("li").ToList();
                if (node_li[1] != null)
                {
                    var node_li_a = node_li[1].Descendants("a").FirstOrDefault();
                    if (node_li_a != null)
                    {
                        var node_li_a_img = node_li_a.Attributes["href"].Value;
                        Console.WriteLine(node_li_a_img.Trim());
                    }
                }
            }
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
