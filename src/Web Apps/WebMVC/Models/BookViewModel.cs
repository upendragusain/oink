using System.Collections.Generic;

namespace WebMVC.Models
{
    public class BookViewModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public List<BookDetailViewModel> Data { get; set; }
    }
}
