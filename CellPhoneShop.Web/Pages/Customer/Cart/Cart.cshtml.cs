using CellPhoneShop.Web.DTOs.Cart.CartDtos;
using CellPhoneShop.Web.DTOs.Cart.CartItemDtos;
using CellPhoneShop.Web.DTOs.Cart.CartDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;

namespace CellPhoneShop.Web.Pages.Customer.Cart
{
    [Authorize]
    public class CartModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CartModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:5001/api/"); // Adjust base URL as needed
        }

        public CartDto Cart { get; set; } = new CartDto();
        [BindProperty]
        public AddToCartDto AddToCartInput { get; set; } = new AddToCartDto();
        [BindProperty]
        public UpdateCartItemDto UpdateCartItemInput { get; set; } = new UpdateCartItemDto();
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            // Assume user ID is retrieved from authentication (e.g., User.Identity)
            int userId = GetUserId(); // Implement this based on your authentication mechanism
            await LoadCartAsync(userId);
        }

        public async Task<IActionResult> OnPostAddToCartAsync()
        {
            int userId = GetUserId();
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"CartItem/add-to-cart?userId={userId}", AddToCartInput);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage();
                }
                ErrorMessage = "Failed to add item to cart.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            await LoadCartAsync(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateCartItemAsync(int cartItemId)
        {
            int userId = GetUserId();
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"CartItem/{cartItemId}", UpdateCartItemInput);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage();
                }
                ErrorMessage = "Failed to update cart item.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            await LoadCartAsync(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveFromCartAsync(int variantId)
        {
            int userId = GetUserId();
            try
            {
                var response = await _httpClient.DeleteAsync($"CartItem/remove-from-cart?userId={userId}&variantId={variantId}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage();
                }
                ErrorMessage = "Failed to remove item from cart.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            await LoadCartAsync(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostClearCartAsync()
        {
            int userId = GetUserId();
            try
            {
                var response = await _httpClient.DeleteAsync($"CartItem/clear-cart/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage();
                }
                ErrorMessage = "Failed to clear cart.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            await LoadCartAsync(userId);
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
                    if (Cart != null && Cart.CartItems != null)
                    {
                        Cart.TotalAmount = Cart.CartItems.Sum(item => (item.Price * (item.Quantity ?? 1)));
                    }
                }
                else
                {
                    ErrorMessage = "Failed to load cart.";
                    Cart = new CartDto { CartItems = new List<CartItemDto>() };
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Cart = new CartDto { CartItems = new List<CartItemDto>() };
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