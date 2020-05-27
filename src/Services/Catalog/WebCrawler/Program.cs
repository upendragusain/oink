using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Infrastructure;
using WebCrawler.Model;

namespace WebCrawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string connectionString = "mongodb://localhost:27017";
            string database = "CatalogDb";
            CatalogDataContext catalogDataContext = new CatalogDataContext(
                connectionString, database);

            var scheduler =
                new AmazonTaskScheduler<AmazonBook>(catalogDataContext);

            var urls = SetUpURLs();

            Console.WriteLine($"Processing with a single thread");
            scheduler.Schedule(urls);

            //Console.WriteLine($"Processing with multiple threads");
            //scheduler.ScheduleWithThreads(urls);

            Console.Read();
        }

        private static List<List<string>> SetUpURLs(
            int threadsToRun = 5, 
            int urlsToProcessPerThread = 10)
        {
            List<List<string>> urls = new List<List<string>>();
            for (int i = 0; i < threadsToRun; i++)
            {
                var threadUrls = new List<string>();
                for (int j = urlsToProcessPerThread * i; j < (urlsToProcessPerThread * i) + urlsToProcessPerThread; j++)
                {
                    var link = $"https://www.amazon.co.uk/s?k=a&i=stripbooks&page="
                        + (j + 1)
                        + "&qid=1590338955&ref=sr_pg_" + (j + 1);
                    threadUrls.Add(link);
                }
                urls.Add(threadUrls);
            }
            return urls;
        }
    }
}
