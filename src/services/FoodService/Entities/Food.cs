using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FoodService.Entities
{
    public class Food
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id{ get; set; }

        public string Name { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }

        public Guid UserId { get; set; }
    }
}
