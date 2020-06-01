namespace Catalog.API.Dto
{
    public class Book : Product
    {
        public string AuthorName { get; set; }

        public string Publisher { get; set; }

        public string Language { get; set; }

        public string ISBN10 { get; set; }

        public string ISBN13 { get; set; }
    }
}
