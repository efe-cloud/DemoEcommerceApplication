using System.Net.Http.Json;
using Ecommerce.client.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Microsoft.Extensions.Logging;

namespace Ecommerce.client.Services
{
    public class OrderApiService : IOrderApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderApiService> _logger;

        public OrderApiService(HttpClient httpClient, ILogger<OrderApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<int> CreateOrderAsync(PaymentDetails paymentDetails)
        {
            _logger.LogInformation("Creating a new order with payment details.");

            var response = await _httpClient.PostAsJsonAsync("api/Orders/create", paymentDetails);
            var rawResponse = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"Order creation response status: {response.StatusCode}");
            _logger.LogInformation($"Order creation response content: {rawResponse}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Order creation failed. Status Code: {response.StatusCode}, Error: {rawResponse}");
                return -1;
            }

            // Try parsing order ID as an integer
            if (int.TryParse(rawResponse, out int orderId) && orderId > 0)
            {
                _logger.LogInformation($"Order created successfully with ID: {orderId}");
                return orderId;
            }

            _logger.LogWarning("Failed to parse order ID from response.");
            return -1;
        }



        public async Task<List<UserOrder>> GetMyOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/Orders/mine");
            if (!response.IsSuccessStatusCode)
                return new List<UserOrder>();

            var orderDtos = await response.Content.ReadFromJsonAsync<List<OrderDto>>();
            if (orderDtos == null) return new List<UserOrder>();

            // Map DTOs to UserOrder models
            var result = new List<UserOrder>();
            foreach (var dto in orderDtos)
            {
                var userOrder = new UserOrder
                {
                    Id = dto.Id,
                    OrderDate = dto.OrderDate,
                    Items = dto.Items.Select(oi => new UserOrderItem
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.ProductName,
                        PriceAtCheckout = oi.PriceAtCheckout,
                        Quantity = oi.Quantity
                    }).ToList(),
                    TotalAmount = dto.TotalAmount // Ensure this is mapped correctly
                };
                result.Add(userOrder);
            }
            return result;
        }

        public async Task<UserOrder> GetOrderByIdAsync(int orderId)
        {
            var response = await _httpClient.GetAsync($"api/Orders/{orderId}");
            if (response.IsSuccessStatusCode)
            {
                var orderDto = await response.Content.ReadFromJsonAsync<OrderDto>();
                if (orderDto != null)
                {
                    return new UserOrder
                    {
                        Id = orderDto.Id,
                        OrderDate = orderDto.OrderDate,
                        Items = orderDto.Items.Select(oi => new UserOrderItem
                        {
                            ProductId = oi.ProductId,
                            ProductName = oi.ProductName,
                            PriceAtCheckout = oi.PriceAtCheckout,
                            Quantity = oi.Quantity
                        }).ToList(),
                        TotalAmount = orderDto.TotalAmount
                    };
                }
            }
            return null;
        }

        public async Task<bool> ConfirmPaymentAsync(int orderId)
        {
            _logger.LogInformation($"Confirming payment for Order ID: {orderId}");

            var response = await _httpClient.PostAsync($"api/Orders/confirm-payment/{orderId}", null);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"ConfirmPaymentAsync failed. Status Code: {response.StatusCode}, Error: {errorContent}");
                return false;
            }

            _logger.LogInformation($"Payment confirmed successfully for Order ID: {orderId}");
            return true;
        }
    }
}
