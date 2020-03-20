using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthorizationService.Entities
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
