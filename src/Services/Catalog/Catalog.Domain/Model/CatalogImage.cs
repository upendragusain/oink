//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
namespace Catalog.API.Model
{
    public class CatalogImage
    {
        public string Url { get; set; }

        public byte[] Content { get; set; }
    }
}