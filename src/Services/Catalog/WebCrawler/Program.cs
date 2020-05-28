using Catalog.API.Model;
using MongoDB.Bson;
using Serilog;
using Serilog.Enrichers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Infrastructure;

namespace WebCrawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = Log.Logger = new LoggerConfiguration()
                 //.MinimumLevel.Debug()
                 .WriteTo.Async(w => w.File("logs.json", 
                 rollingInterval: RollingInterval.Hour, 
                 outputTemplate: 
                 "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} #{ThreadId} {NewLine}{Exception}"))
                 .Enrich.WithThreadId()
                 .CreateLogger();

            string connectionString = "mongodb://localhost:27017";
            string database = "CatalogDb";
            CatalogDataContext catalogDataContext = new CatalogDataContext(
                connectionString, database);

            var scheduler =
                new AmazonTaskScheduler<Book>(catalogDataContext);

            var urls = SetUpURLs();
            int skip = 75*7;//abcdefg
            int take = 75*5;//h,i,j,k,l
            var urls_part = urls.Skip(skip).Take(take).ToList();

            //var urlsProcessed = await scheduler.ScheduleSingleThread(urls_part);
            var urlsProcessed = await scheduler.ScheduleWithSemaphore(urls_part);

            Console.WriteLine($"Processed {urlsProcessed}/{urls_part.Count} urls.");
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
