namespace Catalog.API.Model
{
    public class Book : CatalogItem
    {
        public string AuthorName { get; set; }

        public string Publisher { get; set; }

        public string Language { get; set; }

        public string ISBN10 { get; set; }

        public string ISBN13 { get; set; }
    }
}
