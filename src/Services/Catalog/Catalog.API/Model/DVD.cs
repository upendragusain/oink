namespace Catalog.API.Model
{
    public class DVD : CatalogItem
    {
        public string DirectorName { get; set; }

        public string Region { get; set; }

        public string RunTime { get; set; }
    }
}
