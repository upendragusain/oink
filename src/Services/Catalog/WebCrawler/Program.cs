using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WebCrawler.Amazon;
using WebCrawler.Infrastructure;
using WebCrawler.Model;

namespace WebCrawler
{
    public class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "mongodb://localhost:27017";
            string database = "CatalogDb";
            CatalogDataContext catalogDataContext = new CatalogDataContext(
                connectionString, database);

            var scheduler = 
                new AmazonTaskScheduler<AmazonBook>(catalogDataContext);

            var uris = SetUpURLList();

            Console.WriteLine($"Processing with single thread");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            scheduler.Schedule(uris);
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");

            Console.WriteLine();
            Console.WriteLine($"Processing with multiple threads");
            stopwatch.Restart();
            scheduler.ScheduleWithThreads(uris);
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");

            Console.Read();
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
