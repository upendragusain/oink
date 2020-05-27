using System;
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
                new AmazonTaskScheduler<AmazonBook>(new Crawler(), catalogDataContext);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Task task = Task.Run(async () => await scheduler.Schedule());
            task.Wait();
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");

            Console.Read();
        }
    }
}
