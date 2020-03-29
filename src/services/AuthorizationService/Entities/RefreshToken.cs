using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthorizationService.Entities
{
    public class RefreshToken
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }
    }
}
