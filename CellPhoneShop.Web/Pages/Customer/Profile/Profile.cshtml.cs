using CellPhoneShop.Business.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http;
using CellPhoneShop.Web.DTOs.Account;
using System.Text;
using System.Text.Json;
using CellPhoneShop.Business.DTOs.CartDtos;
using CellPhoneShop.Web.DTOs.Order.OrderDtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace CellPhoneShop.Web.Pages.Customer.Profile
{
    public class ProfileModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty(SupportsGet = true)]
        public UserProfileDto Accounts { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public List<OrderDto> Carts { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public List<OrderDto> CartsComplete { get; set; } = new();


        public int complete = 0, request = 0, shipping = 0;
        public string? usId { get; set; }

        [BindProperty]
        public int? Id { get; set; }

        [BindProperty]
        public ChangePasswordDTO ChangePassword { get; set; }

        public ProfileModel(IHttpClientFactory httpClientFactory)
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
                UserProfileDto result = await client.GetFromJsonAsync<UserProfileDto>("api/User/profile");
                if (result != null)
                {
                    Accounts = result;
                    Console.WriteLine($"data {result.UserId}");
                    if (result.UserId != null)
                    {
                        var carts = await client.GetFromJsonAsync<List<OrderDto>>($"/api/Orders/OrderByUserId/{result.UserId}");
                        if (carts != null)
                        {
                            Carts = carts;
                            foreach (var cart in carts)
                            {

                                switch (cart.Status)
                                {
                                    case 4:
                                        CartsComplete.Add(cart);
                                        break;
                                    case 3:
                                        complete++;
                                        break;
                                    case 2:
                                        shipping++;
                                        break;
                                    case 1:
                                    case 0:
                                        request++;
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Invalid user ID. {result.UserId}");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Could not load user profile.");
                }

            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi (có thể là 401, 500,...)
                ModelState.AddModelError(string.Empty, $"Error fetching accounts: {ex.Message}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAccountAsync()
        {
            var client = _httpClientFactory.CreateClient("API");

            // Lấy token từ claim
            string token = User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }

            // Lấy userId từ claim
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "Không thể xác định người dùng.");
                return Page();
            }

            // Gửi token trong Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            usId = userId;

            // Gửi yêu cầu xóa tài khoản
            var response = await client.DeleteAsync($"/api/User/deleteMyAccount/{userId}");

            if (response.IsSuccessStatusCode)
            {
                // Xóa thành công, đăng xuất người dùng và chuyển về trang đăng nhập
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToPage("/Auth/Login");
            }

            // Xóa thất bại, hiển thị lỗi
            ModelState.AddModelError(string.Empty, "Không thể xóa tài khoản.");
            await OnGetAsync(); // Cập nhật lại dữ liệu trang nếu cần
            return Page();
        }


        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ChangePassword.NewPassword != ChangePassword.ConfirmPassword)
            {
                ModelState.AddModelError("ChangePassword.ConfirmPassword", "Mật khẩu xác nhận không khớp.");
                return Page();
            }

            var client = _httpClientFactory.CreateClient("API");

            string token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestContent = new StringContent(
                JsonSerializer.Serialize(ChangePassword),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PutAsync("api/User/changePass", requestContent); // tùy endpoint backend

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi.";
                return RedirectToPage();
            }

            var error = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = error;
            return Page();
        }


    }
}