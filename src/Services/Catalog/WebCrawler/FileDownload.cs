using System.Net.Http;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class FileDownload : IFileDownload
    {
        public async Task<byte[]> Download(string url)
        {
            HttpClient httpClient = new HttpClient();
            return await httpClient.GetByteArrayAsync(url);
        }
    }
}
