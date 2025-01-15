using System.Collections.Generic;

namespace Ecommerce.library.Models
{
    public class Cart
    {
        public int Id { get; set; }

        // The owner of this cart
        public int UserId { get; set; }
        public User User { get; set; }

        // Navigation for cart items
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
