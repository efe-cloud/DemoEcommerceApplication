using System.Text.Json.Serialization;

namespace Ecommerce.library.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        // Relationship to the Cart
        public int CartId { get; set; }

        [JsonIgnore]
        public Cart Cart { get; set; }

        // The product
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
