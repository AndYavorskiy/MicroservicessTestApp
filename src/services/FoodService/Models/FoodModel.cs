using System;

namespace FoodService.Models
{
    public class FoodModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }

        public Guid UserId { get; set; }
    }
}
