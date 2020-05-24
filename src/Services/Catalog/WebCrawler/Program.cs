using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;

namespace WebCrawler
{
    public class Program
    {
        static void Main(string[] args)
        {
            var url = "https://www.amazon.co.uk/s?k=a&i=stripbooks&ref=nb_sb_noss";
            //var url = "https://www.amazon.co.uk/s/ref=lp_266239_nr_n_0?fst=as%3Aoff&rh=n%3A266239%2Cn%3A%211025612%2Cn%3A349777011&bbn=1025612&ie=UTF8&qid=1590349503&rnid=1025612";

            HttpClient httpClient = new HttpClient();
            var response = httpClient.GetAsync(url);
            var content = response.Result.Content.ReadAsStreamAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(content.Result);

            GetPageLink(htmlDoc);

            string x = @"//a[@href]";
            x = @"//*[contains(concat(' ', normalize-space(@class), ' '), ' a-spacing-none ') and contains(concat(' ', normalize-space(@class), ' '), ' a-section ')]";
            int counter = 1;
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes(x))
            {
                counter++;
                var node_h2 = link.Descendants("h2").FirstOrDefault();
                if (node_h2 != null)
                {
                    //Console.WriteLine($"{counter} : ================================");
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
            string x = @"//a[@href]";
            x = @"//*[contains(concat(' ', normalize-space(@class), ' '), ' s-image ')]";
            int counter = 1;
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes(x))
            {
                counter++;
                //Console.WriteLine($"{counter} : ================================");
                var image_src = link.Attributes["src"].Value;
                Console.WriteLine(image_src.Trim());
                //var image_alt = link.Attributes["alt"].Value;
                //Console.WriteLine(image_alt.Trim());
                counter++;
            }
        }

        //https://www.amazon.co.uk/s?k=a&i=stripbooks
        private static void GetPageLink(HtmlDocument htmlDoc)
        {
            string x = @"//*[contains(concat(' ', normalize-space(@class), ' '), ' a-pagination ')]";
            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes(x))
            {
                var node_li = link.Descendants("li").ToList();
                if (node_li[1] != null)
                {
                    var node_li_a = node_li[1].Descendants("a").FirstOrDefault();
                    if(node_li_a != null)
                    {
                        var node_li_a_img = node_li_a.Attributes["href"].Value;
                        Console.WriteLine(node_li_a_img.Trim());
                    }
                }
            }
        }
    }
}
