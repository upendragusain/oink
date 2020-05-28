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

        //public async Task<List<T>> GetCollectionAsync<T>()
        //{
        //    var booksCollection = _database.GetCollection<T>("Books");
        //    return await booksCollection.Find(new BsonDocument()).ToListAsync();
        //}

        //public async Task GetCollectionAsync<T>()
        //{
        //    var booksCollection = _database.GetCollection<T>("Books");
        //    await booksCollection.Find(new BsonDocument())
        //        .ForEachAsync(async d => await InsertOneAsync(d));
        //}

        //public async Task UpdateOneAsync<T>(string documentId, byte[] image content)
        //{
        //    var booksCollection = _database.GetCollection<T>("Books");
        //    var filter = Builders<T>.Filter.Eq("id", ObjectId.Parse(documentId));
        //    var update = Builders<T>.Update.Set("uri", book);
        //    await booksCollection.UpdateOneAsync(filter, update);


        //    //var document = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();
        //    //await booksCollection.UpdateOneAsync(book);
        //}
    }
}
