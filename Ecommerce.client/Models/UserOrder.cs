using System;
using System.Collections.Generic;

namespace Ecommerce.client.Models
{
    public class UserOrder
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<UserOrderItem> Items { get; set; } = new List<UserOrderItem>();
        public double TotalAmount { get; set; } // Ensure this property exists
    }

    public class UserOrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double PriceAtCheckout { get; set; }
        public int Quantity { get; set; }
    }
}
