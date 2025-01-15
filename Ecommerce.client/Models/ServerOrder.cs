namespace Ecommerce.client.Models
{
    public class ServerOrder
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<ServerOrderItem> Items { get; set; }
    }

    public class ServerOrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double PriceAtCheckout { get; set; }
        public ServerProduct Product { get; set; }
    }

    public class ServerProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
    }
}
