using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Models
{
    public class Course
    {
        [BsonId]//mongoda kullanılan otomatik id veriyor
        [BsonRepresentation(BsonType.ObjectId)]// mongoda tipini belirlemek için. Burdaki tipi ObjectId şeklinde olacak
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]// Decimal şeklinde tutacak veri
        public decimal Price { get; set; }

        public string UserId { get; set; }
        public string Picture { get; set; }

        [BsonRepresentation(BsonType.DateTime)]// Tarih şeklinde veri tutulacak
        public DateTime CreatedTime { get; set; }

        public Feature Feature { get; set; } // bire bir ilişki tutmak için kullanmak için. bire bir ilişki örneği

        [BsonRepresentation(BsonType.ObjectId)] //tipini ObjectId şeklinde tutması
        public string CategoryId { get; set; }

        [BsonIgnore]//mongodb oluştururken bunu gözardı et. yani mongo içinde bir tablo oluşturma
        public Category Category { get; set; } // productları dönerken Category leride dönmek için kullanılyorsun.
    }
}