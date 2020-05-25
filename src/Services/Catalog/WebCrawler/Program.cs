using System;
using System.Diagnostics.Tracing;
using System.Net.Http;
using WebCrawler.Amazon;
using WebCrawler.Model;

namespace WebCrawler
{
    public class Program
    {
        static void Main(string[] args)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://www.amazon.co.uk/s?k=a&i=stripbooks");
            var amazonCrawler = new Crawler(httpClient);
            var books = amazonCrawler.ProcessAsync();
            int counter = 0;
            foreach (var item in books.Result)
            {
                Console.WriteLine($"{++counter}: {item.Title}, {item.Author}, {item.Uri}");
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
