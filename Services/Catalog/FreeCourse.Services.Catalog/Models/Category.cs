using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Models
{
    public class Category
    {
        [BsonId]//mongoda kullanılan otomatik id veriyor
        [BsonRepresentation(BsonType.ObjectId)] // mongoda tipini belirlemek için. Burdaki tipi ObjectId şeklinde olacak
        public string Id { get; set; }

        public string Name { get; set; }
    }
}