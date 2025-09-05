using CellPhoneShop.Web.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CellPhoneShop.Web.Pages.Auth
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public ResetPasswordModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string AgainPassword { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
        
        if(NewPassword == AgainPassword) { 

            var client = _clientFactory.CreateClient("API");

            var dto = new
            {
                Email = this.Email,
                NewPassword = this.NewPassword
            };

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/User/reset-password", content);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản với email này.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi cập nhật mật khẩu.");
                }

                return Page();
            }

                // Xoá OTP đã lưu trong RAM
               ForgotPasswordModel.otpMemory.TryRemove(Email, out _);

            TempData["Success"] = true; // Cho phép hiển thị alert JS
            return Page(); // Không redirect để hiển thị alert
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu nhập lại không giống mật khẩu mới!");
                return Page();
            }
        }
    }
}
