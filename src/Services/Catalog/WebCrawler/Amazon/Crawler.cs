using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WebCrawler.Model;

namespace WebCrawler.Amazon
{
    public class Crawler : ICrawler<AmazonBook>
    {
        private const string XPATH = @"//div[@data-asin]";//data-asin="0702300179"

        public async Task<IEnumerable<AmazonBook>> ProcessAsync(string pageUrl)
        {
            var pageBooks = new List<AmazonBook>();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(pageUrl);
            if (!response.IsSuccessStatusCode)
                return pageBooks;

            var content = await response.Content.ReadAsStreamAsync();
            //if (content.Length < 100)
            //    return pageBooks;

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
                        pageBooks.Add(new AmazonBook()
                        {
                            Title = HttpUtility.HtmlDecode(node_alt.Trim()),
                            Uri = node_src.Trim(),
                            Author = HttpUtility.HtmlDecode(node_author.InnerText.Trim())
                        });
                    }
                }
            }

            return pageBooks;
        }
    }
}
