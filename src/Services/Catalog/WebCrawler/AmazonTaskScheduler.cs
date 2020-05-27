using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

        public void ScheduleWithThreads(
            List<List<string>> urls)
        {
            try
            {
                // Use ConcurrentQueue to enable safe enqueueing from multiple threads.
                var exceptions = new ConcurrentQueue<Exception>();

                Parallel.ForEach(urls, async (urlsToProcessInThisThread) =>
                {
                    try
                    {
                        foreach (var uri in urlsToProcessInThisThread)
                        {
                            //Console.WriteLine($"Processing {uri} on thread {Thread.CurrentThread.ManagedThreadId}");
                            Crawler crawler = new Crawler();
                            var pageBooks = await crawler.ProcessAsync(uri);
                            if (pageBooks != null && pageBooks.Any())
                                await _context.InsertManyAsync(pageBooks);
                        }
                    }
                    // Store the exception and continue with the loop.
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }

                    // Throw the exceptions here after the loop completes.
                    if (exceptions.Count > 0) throw new AggregateException(exceptions);
                });
            }
            catch (AggregateException ae)
            {
                var ignoredExceptions = new List<Exception>();
                // This is where you can choose which exceptions to handle.
                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    if (ex is ArgumentException)
                        Console.WriteLine(ex.Message);
                    else
                        ignoredExceptions.Add(ex);
                }
                if (ignoredExceptions.Count > 0) throw new AggregateException(ignoredExceptions);
            }
        }

        // for testing purposes
        public void Schedule(List<List<string>> urls)
        {
            foreach (var urlsToProcessInThisThread in urls)
            {
                foreach (var url in urlsToProcessInThisThread)
                {
                    Console.WriteLine($"SINGLE Processing {url} on thread {Thread.CurrentThread.ManagedThreadId}");
                    Crawler crawler = new Crawler();
                    var pageBooks = crawler.ProcessAsync(url);

                    if (pageBooks != null && pageBooks.Result.Any())
                        _context.InsertMany(pageBooks.Result);
                }
            }
        }
    }
}
