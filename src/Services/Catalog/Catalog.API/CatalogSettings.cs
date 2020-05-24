using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API
{
    public class CatalogSettings
    {
        public string ConnectionString { get; set; }
        public string MongoConnectionString { get; set; }
        public string MongoDatabase { get; set; }
    }
}
