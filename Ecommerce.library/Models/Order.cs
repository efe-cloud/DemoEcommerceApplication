using System;
using System.Collections.Generic;

namespace Ecommerce.library.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // The owner of the order
        public int UserId { get; set; }
        public User User { get; set; }

        // Navigation property for related items
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
