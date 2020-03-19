using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace UserService.Entities
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

        public bool IsActive { get; set; }
    }
}
