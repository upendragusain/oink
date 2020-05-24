using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Catalog.API.Model
{
    public class CatalogItem
    {
        //designate this property as the document's primary key.
        [BsonId]
        //to allow passing the parameter as type string instead of an ObjectId structure. Mongo handles the conversion from string to ObjectId.
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DepartmentType Department { get; set; }

        public List<CategoryType> Categories { get; set; }

        public List<Uri> Pictures { get; set; }

        public List<CustomerReview> Reviews { get; set; }
    }
}
