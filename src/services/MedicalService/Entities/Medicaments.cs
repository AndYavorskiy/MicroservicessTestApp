using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MedicalService.Entities
{
    public class Medicaments
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }

        public string UserId { get; set; }
    }
}
