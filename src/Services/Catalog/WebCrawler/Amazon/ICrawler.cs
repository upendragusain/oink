using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebCrawler.Amazon
{
    public interface ICrawler<T>
    {
        Task<IEnumerable<T>> ProcessAsync(Uri uri);
    }
}
