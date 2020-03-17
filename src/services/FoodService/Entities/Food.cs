using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FoodService.Entities
{
    public class Food
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }

        public Guid UserId { get; set; }
    }
}
