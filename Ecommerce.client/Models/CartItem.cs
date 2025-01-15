namespace Ecommerce.client.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; } 
        public double TotalPrice => Price * Quantity;
    }
}
