using System;
using System.Collections.Generic;

namespace Ecommerce.client.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public double TotalAmount { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double PriceAtCheckout { get; set; }
        public int Quantity { get; set; }
    }
}
