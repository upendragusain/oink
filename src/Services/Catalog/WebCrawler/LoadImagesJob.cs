//using System;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using WebCrawler.Infrastructure;
//using WebCrawler.Model;

//namespace WebCrawler
//{
//    public class LoadImagesJob
//    {
//        private readonly CatalogDataContext _context;
//        private readonly IFileDownload _fileDownload;

//        public LoadImagesJob(CatalogDataContext context,
//            IFileDownload fileDownload)
//        {
//            _context = context;
//            _fileDownload = fileDownload;
//        }

//        public async Task Run()
//        {
//            //get the books from db
//            var books = await _context.GetCollectionAsync<AmazonBook>();

//            //download image for each book
//            books.for

//            //save data with images
//        }

//        private async Task DownloadImageAndSave(AmazonBook book)
//        {
//            var imageContent = await _fileDownload.Download(book.Uri);
//            _context.InsertOneAsync<AmazonBook>();
//        }
//    }
//}
