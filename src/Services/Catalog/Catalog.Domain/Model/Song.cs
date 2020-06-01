using System;

namespace Catalog.Domain.Model
{
    public class Song : CatalogItem
    {
        public string Artist { get; set; }

        public string Album { get; set; }

        public Genre Genre { get; set; }

        public string Label { get; set; }

        public TimeSpan Time { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
