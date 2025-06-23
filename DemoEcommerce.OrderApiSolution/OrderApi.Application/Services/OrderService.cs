using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using Polly.Registry;
using System.Net.Http.Json;

namespace OrderApi.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrder _orderInterface;
        private readonly HttpClient _productClient;
        private readonly HttpClient _authClient;
        private readonly ResiliencePipelineProvider<string> _resiliencePipeline;

        public OrderService(IOrder orderInterface, IHttpClientFactory httpClientFactory,
            ResiliencePipelineProvider<string> resiliencePipeline)
        {
            _orderInterface = orderInterface;
            _productClient = httpClientFactory.CreateClient("ProductClient");
            _authClient = httpClientFactory.CreateClient("AuthClient");
            _resiliencePipeline = resiliencePipeline;
        }

        // GET PRODUCT
        public async Task<ProductDTO> GetProduct(int productId)
        {
            var getProduct = await _productClient.GetAsync($"/api/products/{productId}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        // GET USER
        public async Task<AppUserDTO> GetUser(int userId)
        {
            var getUser = await _authClient.GetAsync($"/api/authentication/{userId}");
            if (!getUser.IsSuccessStatusCode)
                return null!;

            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user!;
        }

        // GET ORDER DETAILS
        public async Task<OrderDetailsDTO?> GetOrderDetails(int orderId)
        {
            var order = await _orderInterface.FindByIdAsync(orderId);
            if (order is null || order.Id <= 0)
                return null;

            var retryPipeline = _resiliencePipeline.GetPipeline("my-retry-pipeline");

            var productDTO = await retryPipeline.ExecuteAsync(async token =>
                await GetProduct(order.ProductId));

            var appUserDTO = await retryPipeline.ExecuteAsync(async token =>
                await GetUser(order.ClientId));

            // Ndërto objektin e plotë
            return new OrderDetailsDTO(
                order.Id,
                productDTO?.Id ?? 0,
                appUserDTO?.Id ?? 0,
                appUserDTO?.Name ?? "",
                appUserDTO?.Email ?? "",
                appUserDTO?.Address ?? "",
                appUserDTO?.TelephoneNumber ?? "",
                productDTO?.Name ?? "",
                order.PurchaseQuantity,
                productDTO?.Price ?? 0,
                (productDTO?.Price ?? 0) * order.PurchaseQuantity,
                order.OrderedDate
            );
        }

        // GET ORDERS BY CLIENT ID
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            var orders = await _orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if (!orders.Any()) return null!;

            var (_, _orders) = OrderConversion.FromEntity(null, orders);
            return _orders!;
        }
    }
}
