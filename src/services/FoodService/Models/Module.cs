using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoodService.Models
{
    public class Module
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

        public long Id { get; set; }

        public string Name { get; set; }
    }
}
