using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace WebCrawler
{
    public class Program
    {
        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            var response = httpClient.GetAsync("https://www.amazon.co.uk/s?k=a&i=stripbooks&ref=nb_sb_noss");
            var content = response.Result.Content.ReadAsStreamAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(content.Result);

            // And and now queue up all the links on this page
            //@class="class"
            string x = @"//a[@href]";
            //x = @"@class='sg-col-inner'";
            //x = @"//*[contains(concat(' ', normalize-space(@class), ' '), ' a-spacing-none ')]";
            x = @"//*[contains(concat(' ', normalize-space(@class), ' '), ' a-spacing-none ') and contains(concat(' ', normalize-space(@class), ' '), ' a-section ')]";
            // x = @"//*[contains-token(@class, 'a-spacing-none')]";
            int counter = 1;
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes(x))
            {
                counter++;
                var node_h2 = link.Descendants("h2").FirstOrDefault();
                if (node_h2 != null)
                {
                    Console.WriteLine($"{counter} : ================================");
                    var node_h2_a = node_h2.Descendants("a").FirstOrDefault();
                    if (node_h2_a != null)
                    {
                        var node_h2_a_span = node_h2_a.Descendants("span");
                        if (node_h2_a != null)
                        {
                            Console.WriteLine(node_h2_a.InnerText.Trim());
                        }
                    }

                    var authorDiv = link.Descendants("div").FirstOrDefault();
                    if (authorDiv != null)
                    {
                        var author_div_a = authorDiv.Descendants("a").FirstOrDefault();
                        if (author_div_a != null)
                        {
                            Console.WriteLine(author_div_a.InnerText.Trim());
                        }
                    }
                }
            }

            GetImageUrlAndTitle(htmlDoc);

            Console.ReadLine();
        }

        private static void GetImageUrlAndTitle(HtmlDocument htmlDoc)
        {
            // And and now queue up all the links on this page
            //@class="class"
            string x = @"//a[@href]";
            //x = @"@class='sg-col-inner'";
            //x = @"//*[contains(concat(' ', normalize-space(@class), ' '), ' a-spacing-none ')]";
            x = @"//*[contains(concat(' ', normalize-space(@class), ' '), ' s-image ')]";
            // x = @"//*[contains-token(@class, 'a-spacing-none')]";
            int counter = 1;
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes(x))
            {
                counter++;
                Console.WriteLine($"{counter} : ================================");
                var image_src = link.Attributes["src"].Value;
                Console.WriteLine(image_src.Trim());
                var image_alt = link.Attributes["alt"].Value;
                Console.WriteLine(image_alt.Trim());
                counter++;
            }
        }
    }
}
