using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Amazon;
using WebCrawler.Infrastructure;

namespace WebCrawler
{
    public class AmazonTaskScheduler<T>
    {
        private readonly CatalogDataContext _context;

        public AmazonTaskScheduler(
            CatalogDataContext context)
        {
            _context = context;
        }

        public void ScheduleWithThreads(List<Uri> uris)
        {
            Parallel.ForEach(uris, async (uri) =>
            {
                Console.WriteLine($"Processing {uri} on thread {Thread.CurrentThread.ManagedThreadId}");
                Crawler crawler = new Crawler();
                var pageBooks = await crawler.ProcessAsync(uri);
                await _context.InsertManyAsync(pageBooks);
            });

            Console.WriteLine("Processing complete. Press any key to exit.");
        }

        public void Schedule(List<Uri> uris)
        {
            foreach (var uri in uris)
            {
                Console.WriteLine($"Processing {uri} on thread {Thread.CurrentThread.ManagedThreadId}");
                Crawler crawler = new Crawler();
                var pageBooks =   crawler.ProcessAsync(uri);
                _context.InsertManyAsync(pageBooks.Result);
            }
        }
    }
}
