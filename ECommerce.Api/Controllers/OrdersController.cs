using ECommerce.Api.Dtos;
using ECommerce.Api.Services;
using Ecommerce.library.Models;
using Ecommerce.library.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ecommerce.client.Models;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersDbService _ordersService;
        private readonly ICartDbService _cartService; //  Inject Cart Service
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrdersDbService ordersService, ICartDbService cartService, ILogger<OrdersController> logger)
        {
            _ordersService = ordersService;
            _cartService = cartService; //  Assign cart service
            _logger = logger;
        }

        private int GetUserIdFromToken()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                _logger.LogError("Failed to parse user ID from token.");
                throw new UnauthorizedAccessException("Invalid user ID.");
            }
            _logger.LogInformation("User ID {UserId} retrieved from token.", userId);
            return userId;
        }

        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateOrder([FromBody] PaymentDetails paymentDetails)
        {
            var userId = GetUserIdFromToken();
            if (paymentDetails == null)
            {
                _logger.LogWarning("Order creation failed: No payment details provided by User ID {UserId}.", userId);
                return BadRequest("Payment details are required.");
            }

            _logger.LogInformation("User ID {UserId} is attempting to create an order.", userId);

            //  Use `_cartService` instead of `_ordersService` to get cart items
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                _logger.LogWarning("Order creation failed: No items in cart for User ID {UserId}.", userId);
                return BadRequest("No items in cart or user not found.");
            }

            var orderId = await _ordersService.CreateOrderAsync(userId);
            if (orderId <= 0)
            {
                return BadRequest("Order could not be created.");
            }

            _logger.LogInformation("Order created successfully for User ID {UserId} with Order ID {OrderId}.", userId, orderId);
            return Ok(orderId);
        }

        [HttpPost("confirm-payment/{orderId:int}")]
        public async Task<ActionResult<string>> ConfirmPayment(int orderId)
        {
            var userId = GetUserIdFromToken();
            var orders = await _ordersService.GetOrdersByUserAsync(userId);

            if (orders == null || !orders.Any())
            {
                _logger.LogError($"No orders found for user {userId}.");
                return NotFound("No orders found.");
            }

            var order = orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                _logger.LogWarning($"Order {orderId} not found for user {userId}.");
                return NotFound("Order not found.");
            }

            var paymentResult = await _ordersService.ConfirmPaymentAsync(orderId);
            if (paymentResult)
            {
                _logger.LogInformation($"Payment confirmed for order {orderId}.");
                return Ok("Payment confirmed successfully.");
            }

            _logger.LogWarning($"Payment failed for order {orderId}.");
            return BadRequest("Payment confirmation failed.");
        }

        [HttpGet("mine")]
        public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
        {
            var userId = GetUserIdFromToken();

            _logger.LogInformation("Fetching order history for User ID {UserId}.", userId);

            var orders = await _ordersService.GetOrdersByUserAsync(userId);
            if (orders == null || !orders.Any())
            {
                _logger.LogWarning("No orders found for User ID {UserId}.", userId);
                return NotFound("No orders found.");
            }

            var orderDtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Items = o.Items.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    PriceAtCheckout = oi.PriceAtCheckout,
                    Quantity = oi.Quantity
                }).ToList()
            }).ToList();

            _logger.LogInformation("{OrderCount} orders retrieved for User ID {UserId}.", orderDtos.Count, userId);
            return Ok(orderDtos);
        }
    }
}
