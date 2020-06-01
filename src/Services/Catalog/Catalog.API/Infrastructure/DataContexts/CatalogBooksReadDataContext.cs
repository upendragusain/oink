using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.DataContexts
{
    public class CatalogBooksReadDataContext
    {
        private readonly IMongoDatabase _database = null;

        public CatalogBooksReadDataContext(IOptions<CatalogSettings> settings)
        {
            var client = new MongoClient(settings.Value.MongoConnectionString);

            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.MongoDatabase);
            }
        }

        private IMongoCollection<Dto.Book> CatalogData
        {
            get
            {
                return _database.GetCollection<Dto.Book>("Books");
            }
        }

        public async Task<Dto.Book> GetSingleOrDefaultAsync(string documentId)
        {
            ObjectId parsedObjectId;
            ObjectId.TryParse(documentId, out parsedObjectId);

            var filter = Builders<Dto.Book>.Filter
                .Eq("Id", parsedObjectId);

            return await CatalogData.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<long> GetTotalCountAsync(string searchTerm = null)
        {
            FilterDefinition<Dto.Book> filter = !string.IsNullOrWhiteSpace(searchTerm)
                ? Builders<Dto.Book>.Filter
                    .Where(p => p.Name.ToLower()
                    .Contains(searchTerm.ToLower()))
                : new BsonDocument();

            return await CatalogData.CountDocumentsAsync(filter);
        }

        public async Task<IEnumerable<Dto.Book>> GetPageDocumentsAsync(
            int pageSize, int pageIndex, string searchTerm = null)
        {
            FilterDefinition<Dto.Book> filter = !string.IsNullOrWhiteSpace(searchTerm)
                ? Builders<Dto.Book>.Filter
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
