using System;

namespace BasketService.Models
{
    public class BasketCompletePurchaseModel
    {
        public string Id { get; set; }

        public string Amount { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }
    }
}
