using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Amazon;
using WebCrawler.Infrastructure;

namespace WebCrawler
{
    public class AmazonTaskScheduler<T>
    {
        private readonly ICrawler<T> _crawler;
        private readonly CatalogDataContext _context;

        public AmazonTaskScheduler(ICrawler<T> crawler, 
            CatalogDataContext context)
        {
            _crawler = crawler;
            _context = context;
        }

        public async Task Schedule()
        {
            var uris = SetUpURLList();
            foreach (var uri in uris)
            {
                var pageBooks = await _crawler.ProcessAsync(uri);
                await _context.InsertManyAsync(pageBooks);
            }
        }

        private static List<Uri> SetUpURLList()
        {
            List<Uri> urls = new List<Uri>
            {
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=2&qid=1590338955&ref=sr_pg_2"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=3&qid=1590338955&ref=sr_pg_3"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=4&qid=1590338955&ref=sr_pg_4"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=5&qid=1590338955&ref=sr_pg_5"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=6&qid=1590338955&ref=sr_pg_6"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=7&qid=1590338955&ref=sr_pg_7"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=8&qid=1590338955&ref=sr_pg_8"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=9&qid=1590338955&ref=sr_pg_9"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=10&qid=1590338955&ref=sr_pg_10")
            };
            return urls;
        }
    }
}
