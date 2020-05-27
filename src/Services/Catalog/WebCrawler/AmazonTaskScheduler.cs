using System.Collections.Generic;
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

        public void ScheduleWithThreads(
            List<List<string>> urls)
        {
            Parallel.ForEach(urls, (urlsInThread) =>
            {
                foreach (var uri in urlsInThread)
                {
                    //Console.WriteLine($"Processing {uri} on thread {Thread.CurrentThread.ManagedThreadId}");
                    Crawler crawler = new Crawler();
                    var pageBooks =  crawler.ProcessAsync(uri);
                     _context.InsertMany(pageBooks.Result);
                    //Console.WriteLine($"Processed {uri}");
                }
            });
        }

        //public void Schedule(List<Uri> uris)
        //{
        //    foreach (var uri in uris)
        //    {
        //        Console.WriteLine($"Processing {uri} on thread {Thread.CurrentThread.ManagedThreadId}");
        //        Crawler crawler = new Crawler();
        //        var pageBooks =   crawler.ProcessAsync(uri);
        //        _context.InsertManyAsync(pageBooks.Result);
        //    }
        //}
    }
}
