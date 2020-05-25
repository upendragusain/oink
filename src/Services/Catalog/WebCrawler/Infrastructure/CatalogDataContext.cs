using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler.Infrastructure
{
    //public class CatalogDataContext
    //{
    //    private readonly IMongoDatabase _database = null;

    //    public CatalogReadDataContext(string settings)
    //    {
    //        var client = new MongoClient(settings.Value.MongoConnectionString);

    //        if (client != null)
    //        {
    //            _database = client.GetDatabase(settings.Value.MongoDatabase);
    //        }
    //    }

    //    public IMongoCollection<CatalogItem> MarketingData
    //    {
    //        get
    //        {
    //            return _database.GetCollection<CatalogItem>("CatalogReadDataContext");
    //        }
    //    }
    //}
}
