using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Console.WriteLine($"Processing with multiple threads");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Execute the antecedent.
            Task taskA = Task.Run(() => scheduler.ScheduleWithThreads(urls));

            // Execute the continuation when the antecedent finishes.
            await taskA.ContinueWith(antecedent =>
            {
                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");

                Console.Read();
            });
        }

        private static List<List<string>> SetUpURLs(int threadCount = 5, int pagesPerThread = 10)
        {
            List<List<string>> urls = new List<List<string>>();
            for (int i = 0; i < threadCount; i++)
            {
                var threadUrls = new List<string>();
                for (int j = pagesPerThread * i; j < (pagesPerThread * i) + pagesPerThread; j++)
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
