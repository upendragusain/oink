using Catalog.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Infrastructure
{
    public class CatalogReadDataContext
    {
        private readonly IMongoDatabase _database = null;

        public CatalogReadDataContext(IOptions<CatalogSettings> settings)
        {
            var client = new MongoClient(settings.Value.MongoConnectionString);

            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.MongoDatabase);
            }
        }

        public IMongoCollection<CatalogItem> MarketingData
        {
            get
            {
                return _database.GetCollection<CatalogItem>("Books");
            }
        }


    }
}
