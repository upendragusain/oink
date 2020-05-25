using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawler.Amazon;
using WebCrawler.Model;

namespace WebCrawler
{
    public class Program
    {
        static void Main(string[] args)
        {
            var uriList = new List<Uri>()
            {
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=2&qid=1590338955&ref=sr_pg_2"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=3&qid=1590338955&ref=sr_pg_3"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=4&qid=1590338955&ref=sr_pg_4"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=5&qid=1590338955&ref=sr_pg_5"),

                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=6&qid=1590338955&ref=sr_pg_6"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=7&qid=1590338955&ref=sr_pg_7"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=8&qid=1590338955&ref=sr_pg_8"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=9&qid=1590338955&ref=sr_pg_9"),
                new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks&page=10&qid=1590338955&ref=sr_pg_10"),
            };

            var uri = new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks");
            ConcurrentBag<AmazonBook> _concurrentBooksCollection = new ConcurrentBag<AmazonBook>();

            //todo
            //An item with the same key has already been added. Key: The Night Fire: The Brand New Ballard and Bosch Thriller (Ballard &amp; Bosch 2)
            Parallel.ForEach(uriList, (uri) =>
            {
                var amazonCrawler = new Crawler(new HttpClient());
                var books = amazonCrawler.ProcessAsync(uri);
                Console.WriteLine(uri);
                foreach (var item in books.Result)
                {
                    Console.WriteLine($"{item.Title}, {item.Author}, {item.Uri}");
                    _concurrentBooksCollection.Add(item);
                }
                Console.WriteLine("----------------------------------------------------------------");
            });

            //Task[] taskArray = new Task[uriList.Count];
            //for (int i = 0; i < taskArray.Length-1; i++)
            //{
            //    taskArray[i] = Task.Factory.StartNew(() =>
            //    {
            //        var amazonCrawler = new Crawler(new HttpClient());
            //        var books = amazonCrawler.ProcessAsync(uriList[i]);
            //        foreach (var item in books.Result)
            //        {
            //            Console.WriteLine($"Task: {i} - {item.Title}, {item.Author}, {item.Uri}");
            //            //Console.WriteLine();
            //            _concurrentBooksCollection.Add(item);
            //        }
            //    });
            //}
            //Task.WaitAll(taskArray);

            Console.ReadLine();
        }
    }
}
