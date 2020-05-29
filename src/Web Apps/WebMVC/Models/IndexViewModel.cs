using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class IndexViewModel
    {
        public IEnumerable<BookDetailViewModel> Books { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
