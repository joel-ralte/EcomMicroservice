using Cart.Models;
using Cart.Services;
using Cart.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cart.Controllers
{
    [ApiController]
    [Route("api/v1/cart")]
    public class CartController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ICartService _cartService;

        public CartController(HttpClient httpClient, ICartService cartService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5203/api/v1/");
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDetails>>> GetAllCartItems()
        {
            var cartItems = await _cartService.GetAllCartItems();
            return Ok(cartItems);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<IEnumerable<CartDetails>>> GetCartDetailsByOrderId(int orderId)
        {
            var cartDetails = await _cartService.GetCartDetailsByOrderId(orderId);
            if (cartDetails == null || !cartDetails.Any())
            {
                return NotFound("No cart details found for the specified order ID.");
            }
            return Ok(cartDetails);
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart([FromBody] CartDetails cartDetails)
        {
            var getProductTask = _httpClient.GetAsync($"products/{cartDetails.ProductId}");
            var response = await getProductTask;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest($"Failed to retrieve product details for product ID: {cartDetails.ProductId}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var productDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductViewModel>(content);

            var totalPrice = productDetails.Price * cartDetails.Quantity;

            cartDetails.ProductName = productDetails.Name;
            cartDetails.Price = productDetails.Price;

            await _cartService.AddToCart(cartDetails);

            return Ok($"Product added to cart successfully. Total price for Product: {totalPrice}");
        }

    }
}
