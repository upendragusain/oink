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
        private static SemaphoreSlim semaphore;

        public AmazonTaskScheduler(
            CatalogDataContext context)
        {
            _context = context;
        }

        public async Task ScheduleWithSemaphore(List<string> urls)
        {
            // Create the semaphore.
            semaphore = new SemaphoreSlim(5, 5);
            List<Task> trackedTasks = new List<Task>();

            foreach (var url in urls)
            {
                await semaphore.WaitAsync();

                trackedTasks.Add(Task.Run(async () =>
                {
                    // Each task begins by requesting the semaphore.
                    //Debug.WriteLine($"{Task.CurrentId}, url {url}");
                    try
                    {
                        Crawler crawler = new Crawler();
                        var pageBooks = await crawler.ProcessAsync(url);
                        if (pageBooks != null && pageBooks.Any())
                            await _context.InsertManyAsync(pageBooks);
                    }
                    catch (System.Exception)
                    {
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(trackedTasks);
        }

        //public void ScheduleWithThreads(
        //    List<List<string>> urls)
        //{
        //    try
        //    {
        //        // Use ConcurrentQueue to enable safe enqueueing from multiple threads.
        //        var exceptions = new ConcurrentQueue<Exception>();
        //        double maxExceptionToleranceNumber = urls.Count() * urls[0].Count * 0.2;

        //        Parallel.ForEach(urls, async (urlsToProcessInThisThread) =>
        //        {
        //            foreach (var uri in urlsToProcessInThisThread)
        //            {
        //                try
        //                {
        //                    //Console.WriteLine($"Processing {uri} on thread {Thread.CurrentThread.ManagedThreadId}");
        //                    Crawler crawler = new Crawler();
        //                    var pageBooks = await crawler.ProcessAsync(uri);
        //                    if (pageBooks != null && pageBooks.Any())
        //                        await _context.InsertManyAsync(pageBooks);
        //                }
        //                catch (Exception e)
        //                {
        //                    exceptions.Enqueue(e);
        //                    if (exceptions.Count > maxExceptionToleranceNumber) throw new AggregateException(exceptions);
        //                }
        //            }
        //        });
        //    }
        //    catch (AggregateException ae)
        //    {
        //        var ignoredExceptions = new List<Exception>();
        //        // This is where you can choose which exceptions to handle.
        //        foreach (var ex in ae.Flatten().InnerExceptions)
        //        {
        //            if (ex is XPathNotFoundException)
        //                Console.WriteLine(ex.Message);
        //            else
        //                ignoredExceptions.Add(ex);
        //        }
        //        if (ignoredExceptions.Count > 0) throw new AggregateException(ignoredExceptions);
        //    }
        //}

        //// for testing purposes
        //public void Schedule(List<List<string>> urls)
        //{
        //    foreach (var urlsToProcessInThisThread in urls)
        //    {
        //        foreach (var url in urlsToProcessInThisThread)
        //        {
        //            Console.WriteLine($"SINGLE Processing {url} on thread {Thread.CurrentThread.ManagedThreadId}");
        //            Crawler crawler = new Crawler();
        //            var pageBooks = crawler.ProcessAsync(url);

        //            if (pageBooks != null && pageBooks.Result.Any())
        //                _context.InsertMany(pageBooks.Result);
        //        }
        //    }
        //}
    }
}
