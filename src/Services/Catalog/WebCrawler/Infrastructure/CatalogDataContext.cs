using Catalog.API.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using WebCrawler.Model;

namespace WebCrawler.Infrastructure
{
    public class CatalogDataContext
    {
        private readonly IMongoDatabase _database = null;

        public CatalogDataContext(
            string mongoDatabase)
        {
            var client = new MongoClient();

            if (client != null)
            {
                _database = client.GetDatabase(mongoDatabase);
            }
        }

        public async void InsertManyAsync(IEnumerable<AmazonBook> books)
        {
            var booksCollection = _database.GetCollection<AmazonBook>("Books");
            await booksCollection.InsertManyAsync(books);
        }
    }
}
