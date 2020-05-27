using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebCrawler.Infrastructure
{
    public class CatalogDataContext
    {
        private readonly IMongoDatabase _database = null;

        public CatalogDataContext(
            string connectionString,
            string database)
        {
            var client = new MongoClient(connectionString);
            if (client != null)
            {
                _database = client.GetDatabase(database);
            }
        }

        public async Task InsertManyAsync<T>(IEnumerable<T> books)
        {
            var booksCollection = _database.GetCollection<T>("Books");
            await booksCollection.InsertManyAsync(books);
        }

        public void InsertMany<T>(IEnumerable<T> books)
        {
            var booksCollection = _database.GetCollection<T>("Books");
            booksCollection.InsertMany(books);
        }
    }
}
