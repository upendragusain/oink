using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using Catalog.Domain.Model;

namespace Catalog.API.Dto
{
    public class Product
    {
        [BsonIgnoreIfDefault]
        //the serializer will convert the ObjectId to a string when reading data from the database and will convert the string back to an ObjectId when writing data to the database (the string value must be a valid ObjectId). 
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DepartmentType Department { get; set; }

        public List<MediaContent> Media { get; set; }

        public List<CustomerReview> Reviews { get; set; }
    }
}
