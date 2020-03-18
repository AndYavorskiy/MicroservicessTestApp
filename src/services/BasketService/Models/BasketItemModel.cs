﻿using BasketService.Enums;
using System;

namespace BasketService.Models
{
    public class BasketItemModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public BasketItemType ItemType { get; set; }

        public DateTimeOffset DateCreated { get; set; }
        
        public DateTimeOffset? ExpirationDate { get; set; }

        public Guid UserId { get; set; }

        public string ExistinctEntityId { get; set; }
    }
}
