using System.Threading.Tasks;

namespace WebCrawler
{
    public interface IFileDownload
    {
        Task<byte[]> Download(string url);
    }
}