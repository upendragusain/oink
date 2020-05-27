using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

            //Console.WriteLine($"Processing with a single thread");
            //scheduler.Schedule(urls);

            //Console.WriteLine($"Processing with multiple threads");
            //scheduler.ScheduleWithThreads(urls);

            //urls = urls.Take(10).ToList();
            await scheduler.ScheduleWithSemaphore(urls);

            Console.Read();
        }

        private static List<string> SetUpURLs(
            int searchPageTotalCount = 75)
        {
            var alphabet = Enumerable.Range('a', 26).ToList();
            var allUrls = new List<string>();
            for (int i = 0; i < 26; i++)
            {
                for (int j = 1; j <= searchPageTotalCount; j++)
                {
                    var link = $"https://www.amazon.co.uk/s?k=" 
                        + Convert.ToChar(alphabet[i]).ToString() 
                        + "&i=stripbooks&page="
                        + j
                        + "&qid=1590338955&ref=sr_pg_" 
                        + j;
                    allUrls.Add(link);
                }
            }

            return allUrls;
        }
    }
}
