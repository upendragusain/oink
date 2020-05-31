using Catalog.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure
{
    public class BookReadDataContext
    {
        private readonly IMongoDatabase _database = null;

        public BookReadDataContext(IOptions<CatalogSettings> settings)
        {
            var client = new MongoClient(settings.Value.MongoConnectionString);

            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.MongoDatabase);
            }
        }

        public IMongoCollection<Book> CatalogData
        {
            get
            {
                return _database.GetCollection<Book>("Books");
            }
        }

        public async Task<Book> GetSingleOrDefaultAsync(string documentId)
        {
            ObjectId parsedObjectId;
            ObjectId.TryParse(documentId, out parsedObjectId);

            var filter = Builders<Book>.Filter
                .Eq("Id", parsedObjectId);

            return await CatalogData.Find(filter).FirstOrDefaultAsync();
        }

        private async Task<List<Book>> GetAllDocumentsAsync()
        {
            return await CatalogData.Find(new BsonDocument())
                .ToListAsync();
        }

        public async Task<long> GetAllDocumentsCountAsync(string searchTerm = null)
        {
            FilterDefinition<Book> filter = !string.IsNullOrWhiteSpace(searchTerm)
                ? Builders<Book>.Filter
                    .Where(p => p.Name.ToLower()
                    .Contains(searchTerm.ToLower()))
                : new BsonDocument();

            return await CatalogData.CountDocumentsAsync(filter);
        }

        public async Task<List<Book>> GetDocumentsForAPage(
            int pageSize, int pageIndex, string searchTerm = null)
        {
            FilterDefinition<Book> filter = !string.IsNullOrWhiteSpace(searchTerm)
                ? Builders<Book>.Filter
                    .Where(p => p.Name.ToLower()
                    .Contains(searchTerm.ToLower()))
                : new BsonDocument();

            return await CatalogData.Find(filter)
                    .Skip(pageSize * (pageIndex - 1))
                    .Limit(pageSize)
                    .ToListAsync();
        }
    }
}
