using ECommerce.Api.Data;
using Ecommerce.library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace ECommerce.Api.Services
{
    public class OrdersDbService : IOrdersDbService
    {
        private readonly AppDbScope _db;
        private readonly ICartDbService _cartService;
        private readonly ILogger<OrdersDbService> _logger;

        public OrdersDbService(AppDbScope db, ICartDbService cartService, ILogger<OrdersDbService> logger)
        {
            _db = db;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<int> CreateOrderAsync(int userId)
        {
            _logger.LogInformation("Attempting to create an order for User ID: {UserId}", userId);

            // ✅ Ensure user has a cart
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null || cart.Items.Count == 0)
            {
                _logger.LogWarning("Order creation failed: No items in cart for User ID {UserId}.", userId);
                return -1; // Return -1 if no cart items exist
            }

            // ✅ Proceed with order creation
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItem>() // Initialize the Items list
            };

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    PriceAtCheckout = cartItem.Product.Price
                };
                order.Items.Add(orderItem);
            }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // ✅ Clear user’s cart
            await _cartService.ClearCartAsync(userId);

            _logger.LogInformation("Order successfully created for User ID {UserId} with Order ID {OrderId}.", userId, order.Id);
            return order.Id;
        }


        public async Task<List<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _db.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        // NEW: Simulate payment confirmation
        public async Task<bool> ConfirmPaymentAsync(int orderId)
        {
            _logger.LogInformation("Confirming payment for Order ID: {OrderId}", orderId);

            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                _logger.LogError("Payment confirmation failed: Order {OrderId} not found.", orderId);
                return false;
            }

            // ✅ Simulate payment confirmation
            _logger.LogInformation("Payment confirmed for Order ID: {OrderId}.", orderId);
            return true;
        }

    }
}
