using System;

namespace FoodService.Models
{
    public class BasketItemModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }

        public string UserId { get; set; }
    }
}
