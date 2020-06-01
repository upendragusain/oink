using System;
using System.Collections.Generic;

namespace Catalog.Domain.Model
{
    public class Movie : CatalogItem
    {
        public string Director { get; set; }

        public string Region { get; set; }

        public string RunTime { get; set; }

        public List<string> Stars { get; set; }

        public string Language { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
