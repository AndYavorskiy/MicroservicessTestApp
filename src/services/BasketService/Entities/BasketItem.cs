using BasketService.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BasketService.Entities
{
    public class BasketItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Amount { get; set; }

        public string Hint { get; set; }

        public string Description { get; set; }

        public BasketItemType ItemType { get; set; }

        public DateTimeOffset DateCreated { get; set; }
        
        public DateTimeOffset? ExpirationDate { get; set; }

        public string UserId { get; set; }

        public string ExistinctEntityId { get; set; }
    }
}
