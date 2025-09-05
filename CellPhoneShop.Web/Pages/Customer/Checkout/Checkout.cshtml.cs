using CellPhoneShop.Web.DTOs.Order.OrderDtos;
using CellPhoneShop.Web.DTOs.Cart.CartDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace CellPhoneShop.Web.Pages.Customer.Checkout
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public CheckoutModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:5001/api/"); // Adjust as needed
        }

        [BindProperty]
        public CheckoutFormDto OrderInput { get; set; } = new CheckoutFormDto();
        public CartDto Cart { get; set; } = new CartDto();
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            int userId = GetUserId();
            await LoadCartAsync(userId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int userId = GetUserId();
            await LoadCartAsync(userId);
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill in all required fields.";
                return Page();
            }
            try
            {
                // Create CreateOrderDto from form data
                var createOrderDto = new CellPhoneShop.Business.DTOs.OrderDtos.CreateOrderDto
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    TotalAmount = Cart?.TotalAmount ?? 0,
                    Status = 1, // Pending status
                    PaymentMethod = 1, // Default payment method
                    Note = OrderInput.Note,
                    AddressDetail = OrderInput.Address,
                    OrderDetails = Cart?.CartItems?.Select(item => new CellPhoneShop.Business.DTOs.OrderDetailDtos.CreateOrderDetailDto
                    {
                        VariantId = item.VariantId ?? 0,
                        Quantity = item.Quantity ?? 1,
                        Price = item.Price
                    }).ToList() ?? new List<CellPhoneShop.Business.DTOs.OrderDetailDtos.CreateOrderDetailDto>()
                };

                var response = await _httpClient.PostAsJsonAsync($"Order", createOrderDto);
                if (response.IsSuccessStatusCode)
                {
                    // Clear cart after successful order
                    await _httpClient.DeleteAsync($"CartItem/clear-cart/{userId}");
                    return RedirectToPage("/Customer/pageThanks/pageThanks");
                }
                ErrorMessage = "Failed to place order.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }

        private async Task LoadCartAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Cart/user/{userId}/active");
                if (response.IsSuccessStatusCode)
                {
                    Cart = await response.Content.ReadFromJsonAsync<CartDto>();
                }
                else
                {
                    Cart = new CartDto();
                }
            }
            catch
            {
                Cart = new CartDto();
            }
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("User ID not found or invalid.");
            }
            return userId;
        }
    }
}
