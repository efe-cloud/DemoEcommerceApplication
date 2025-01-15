using Ecommerce.library.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Data
{
    public class AppDbScope : DbContext
    {
        public AppDbScope(DbContextOptions<AppDbScope> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        // NEW:
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Force lowercase table names in PostgreSQL
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Cart>().ToTable("carts");
            modelBuilder.Entity<CartItem>().ToTable("cartitems");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<OrderItem>().ToTable("orderitems");

            // USER
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Email).HasColumnName("email");
                entity.Property(u => u.PasswordHash).HasColumnName("passwordhash");
                entity.Property(u => u.PasswordSalt).HasColumnName("passwordsalt");
            });

            // PRODUCT
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.CategoryId).HasColumnName("categoryid");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.Image).HasColumnName("image");
            });

            // CATEGORY
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Image).HasColumnName("image");
            });

            // Relationship product -> category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // CART
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("userid");
            });

            // CART ITEM
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CartId).HasColumnName("cartid");
                entity.Property(e => e.ProductId).HasColumnName("productid");
                entity.Property(e => e.Quantity).HasColumnName("quantity");

                // Relationship to product
                entity.HasOne(ci => ci.Product)
                      .WithMany()
                      .HasForeignKey(ci => ci.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relationship to cart
                entity.HasOne(ci => ci.Cart)
                      .WithMany(c => c.Items)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ORDER
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("userid");
                entity.Property(e => e.OrderDate).HasColumnName("orderdate");
            });

            // ORDER ITEM
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.OrderId).HasColumnName("orderid");
                entity.Property(e => e.ProductId).HasColumnName("productid");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.PriceAtCheckout).HasColumnName("priceatcheckout");

                // Relationship to product
                entity.HasOne(oi => oi.Product)
                      .WithMany()
                      .HasForeignKey(oi => oi.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relationship to order
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.Items)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ----------------------------------------------------------------
            // SEED DATA
            // ----------------------------------------------------------------
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "electronics", Image = "electronics.png" },
                new Category { Id = 2, Name = "clothing", Image = "clothing.png" },
                new Category { Id = 3, Name = "home & kitchen", Image = "home_kitchen.png" },
                new Category { Id = 4, Name = "books", Image = "books.png" },
                new Category { Id = 5, Name = "sports", Image = "sports.png" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "laptop", Description = "Gaming laptop with RTX 3060", CategoryId = 1, Price = 1500.00, Quantity = 10, Image = "laptop.png" },
                new Product { Id = 2, Name = "smartphone", Description = "Latest 5G smartphone", CategoryId = 1, Price = 999.99, Quantity = 25, Image = "smartphone.png" },
                new Product { Id = 3, Name = "t-shirt", Description = "Cotton round neck t-shirt", CategoryId = 2, Price = 19.99, Quantity = 50, Image = "tshirt.png" },
                new Product { Id = 4, Name = "jeans", Description = "Slim fit denim jeans", CategoryId = 2, Price = 49.99, Quantity = 30, Image = "jeans.png" },
                new Product { Id = 5, Name = "blender", Description = "500W powerful blender", CategoryId = 3, Price = 79.99, Quantity = 15, Image = "blender.png" },
                new Product { Id = 6, Name = "cookware set", Description = "Non-stick cookware set of 5", CategoryId = 3, Price = 120.00, Quantity = 8, Image = "cookware.png" },
                new Product { Id = 7, Name = "fiction novel", Description = "Best-selling fiction book", CategoryId = 4, Price = 14.99, Quantity = 40, Image = "fiction.png" },
                new Product { Id = 8, Name = "notebook", Description = "Lined notebook for school", CategoryId = 4, Price = 5.99, Quantity = 100, Image = "notebook.png" },
                new Product { Id = 9, Name = "tennis racket", Description = "Professional-grade tennis racket", CategoryId = 5, Price = 129.99, Quantity = 12, Image = "tennis.png" },
                new Product { Id = 10, Name = "yoga mat", Description = "Non-slip eco-friendly yoga mat", CategoryId = 5, Price = 39.99, Quantity = 20, Image = "yoga.png" },
                new Product { Id = 34, Name = "Dubai Chocolate", Description = "tasty and does not include added sugar", CategoryId = 3, Price = 5.99, Quantity = 220, Image = "dubaichocolate.png" },
                new Product { Id = 37, Name = "usb128gb", Description = "store your things in it efficently", CategoryId = 1, Price = 9.99, Quantity = 230, Image = "usb128gb.png" },
                new Product { Id = 11, Name = "Smart Watch", Description = "With phone call support", CategoryId = 1, Price = 445.99, Quantity = 120, Image = "smartwatch.png" },
                new Product { Id = 12, Name = "Sport Hat", Description = "Good for running", CategoryId = 2, Price = 19.99, Quantity = 530, Image = "sporthat.png" }
            );
        }
    }
}
