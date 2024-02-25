using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Customer.Services;
using Customer.Models;
using Customer.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("api/v1/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly HttpClient _httpClient;

        public CustomersController(ICustomerService customerService, HttpClient httpClient)
        {
            _customerService = customerService;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("http://localhost:5203/api/v1/");
        }

        [HttpGet("{id}")]
        public ActionResult<CustomerDetails> GetCustomerById(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerDetails>> GetAllCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            return Ok(customers);
        }

        [HttpPost]
        public ActionResult<CustomerDetails> CreateCustomer([FromBody] CustomerDetails customer)
        {
            _customerService.CreateCustomer(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        [HttpPost("buy-product")]
        public ActionResult BuyProduct([FromBody] ProductViewModel productViewModel)
        {
            var getProductTask = _httpClient.GetAsync($"products/{productViewModel.Id}");
            getProductTask.Wait();

            var response = getProductTask.Result;

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to retrieve product details from Catalog API.");
            }

            var content = response.Content.ReadAsStringAsync();
            content.Wait();

            var productDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductViewModel>(content.Result);

            var totalPrice = productDetails.Price * productViewModel.Quantity;

            return Ok($"Product bought successfully. Total price: {totalPrice}");
        }



        [HttpPost("add-to-cart")]
        public async Task<ActionResult> AddToCart([FromBody] OrderViewModel orderViewModel)
        {
            var cartApiUrl = "http://localhost:5011/api/v1/cart";

            var content = new StringContent(JsonConvert.SerializeObject(orderViewModel), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(cartApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to add product to cart.");
            }

            return Ok("Product added to cart successfully.");
        }


    }
}
