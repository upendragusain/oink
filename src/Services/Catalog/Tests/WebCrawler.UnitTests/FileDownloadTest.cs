using System;
using System.Threading.Tasks;
using Xunit;

namespace WebCrawler.UnitTests
{
    public class FileDownloadTest
    {
        [Fact]
        public async Task AFileIsDownloadedOnFileDownload()
        {
            var url = "https://m.media-amazon.com/images/I/61-gczrW31L._AC_UY218_.jpg";
            FileDownload fileDownload = new FileDownload();
            var file = await fileDownload.Download(url);

            Assert.True(file.Length > 0);
        }
    }
}
