using CellPhoneShop.Business.DTOs;
using CellPhoneShop.Web.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Admin.Account
{
    [Authorize(Roles = "Admin,Staff")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<UserProfileDto> Accounts { get; set; } = new();

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("API");

            // Lấy token từ cookie (hoặc từ nơi bạn lưu token)
            string token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                // Chưa có token -> redirect đến trang đăng nhập
                return RedirectToPage("/Auth/Login");
            }

            // Thêm Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var result = await client.GetFromJsonAsync<List<UserProfileDto>>("/api/User/Alluser");
                if (result != null)
                {
                    Accounts = result;
                }
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi (có thể là 401, 500,...)
                ModelState.AddModelError(string.Empty, $"Error fetching accounts: {ex.Message}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int userId)
        {
            var client = _httpClientFactory.CreateClient("API");

            string token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"/api/User/deleteUser/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }

            ModelState.AddModelError(string.Empty, "Cannot delete this account.");
            await OnGetAsync();
            return Page();
        }
    }
} 