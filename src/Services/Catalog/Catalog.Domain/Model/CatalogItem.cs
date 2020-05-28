using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Catalog.API.Model
{
    public class CatalogItem
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DepartmentType Department { get; set; }

        public List<CategoryType> Categories { get; set; }

        public List<CatalogImage> Images { get; set; }

        public List<CustomerReview> Reviews { get; set; }
    }
}
