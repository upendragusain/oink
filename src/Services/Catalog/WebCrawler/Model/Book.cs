using Catalog.Domain.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WebCrawler.Model
{
    public class Book
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

        public string AuthorName { get; set; }

        public string Publisher { get; set; }

        public string Language { get; set; }

        public string ISBN10 { get; set; }

        public string ISBN13 { get; set; }
    }
}
