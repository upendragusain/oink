﻿using Serilog;
using System;
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
            semaphore = new SemaphoreSlim(4, 4);
            List<Task> trackedTasks = new List<Task>();

            foreach (var url in urls)
            {
                await semaphore.WaitAsync();

                trackedTasks.Add(Task.Run(async () =>
                {
                    // Each task begins by requesting the semaphore.
                    try
                    {
                        //get the page books
                        Crawler crawler = new Crawler();

                        Log.Information("Before Processing page {0}", url);
                        var pageBooks = await crawler.ProcessAsync(url);
                        Log.Information("After Processing page {0}", url);

                        if (pageBooks != null && pageBooks.Any())
                        {
                            //download images for the book url
                            foreach (var book in pageBooks)
                            {
                                Log.Information("Before Processing book image url {0}", book.Id);
                                var firstImage = book.Images.FirstOrDefault();
                                if (firstImage != null)
                                {
                                    FileDownload fileDownload = new FileDownload();
                                    firstImage.Content = await fileDownload.Download(firstImage.Url);
                                    Log.Information("After Processing book image url {0}", firstImage.Url);
                                }
                            }

                            //finally save to db
                            Log.Information("Before saving to db {0}", url);
                            await _context.InsertManyAsync(pageBooks);
                            Log.Information("After saving to db {0}", url);
                        }
                    }
                    catch (System.Exception)
                    {
                    }
                    finally
                    {
                        Log.Information("calling semaphore.Release();");
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

        // for testing purposes
        public async Task Schedule(List<string> urls)
        {
            foreach (var url in urls)
            {
                try
                {
                    //get the page books
                    Crawler crawler = new Crawler();
                    Console.WriteLine($"Processing {url}");
                    Log.Information("Processing page {0}", url);
                    var pageBooks = await crawler.ProcessAsync(url);

                    if (pageBooks != null && pageBooks.Any())
                    {
                        //download images for the book url
                        Log.Information("Processing page books images ...");
                        foreach (var book in pageBooks)
                        {
                            var firstImage = book.Images.FirstOrDefault();
                            if (firstImage != null)
                            {
                                FileDownload fileDownload = new FileDownload();
                                firstImage.Content = await fileDownload.Download(firstImage.Url);
                            }
                        }

                        //finally save to db
                        await _context.InsertManyAsync(pageBooks);
                        Log.Information("Saved books to db");
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error("{0}", ex);
                }
            }
        }
    }
}
