using HtmlAgilityPack;
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
        private const string XPATH = @"//div[@data-asin]";//data-asin="0702300179"

        public Crawler()
        {
        }

        public async Task<IEnumerable<AmazonBook>> ProcessAsync(Uri pageUrl)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(pageUrl).ConfigureAwait(false);
            var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(content);

            var pageBooks = new List<AmazonBook>();
            foreach (HtmlNode htmlNode in htmlDocument.DocumentNode.SelectNodes(XPATH))
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
                            Title = node_alt.Trim(),
                            Uri = node_src.Trim(),
                            Author = node_author.InnerText.Trim()
                        });
                    }
                }
            }

            return pageBooks;
        }
    }
}
