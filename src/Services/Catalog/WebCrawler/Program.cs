using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Amazon;
using WebCrawler.Infrastructure;

namespace WebCrawler
{
    public class Program
    {
        static void Main(string[] args)
        {
            var uriList = new List<Uri>()
            {
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=2&qid=1590338955&ref=sr_pg_2"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=3&qid=1590338955&ref=sr_pg_3"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=4&qid=1590338955&ref=sr_pg_4"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=5&qid=1590338955&ref=sr_pg_5"),

                //new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=6&qid=1590338955&ref=sr_pg_6"),
                //new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=7&qid=1590338955&ref=sr_pg_7"),
                //new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=8&qid=1590338955&ref=sr_pg_8"),
                //new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=9&qid=1590338955&ref=sr_pg_9"),
                //new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=10&qid=1590338955&ref=sr_pg_10"),
            };

            string database = "CatalogDb";
            CatalogDataContext catalogDataContext = new CatalogDataContext(database);

            var amazonCrawler = new Crawler();
            Task.Factory.StartNew(async () =>
            {
                foreach (var uri in uriList)
                {
                    var pageBooks = await amazonCrawler.ProcessAsync(uri);
                    catalogDataContext.InsertManyAsync(pageBooks);
                }
            });

            Console.ReadLine();
        }
    }
}
