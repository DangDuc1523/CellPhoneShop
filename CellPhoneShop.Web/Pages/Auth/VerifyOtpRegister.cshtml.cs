using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using CellPhoneShop.Web.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CellPhoneShop.Web.Pages.Auth
{
    public class VerifyOtpRegisterModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public VerifyOtpRegisterModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public RegisterRequest RegisterRequest { get; set; }

        [TempData]
        public string RegisterRequestJson { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập mã OTP.")]
        public string OtpInput { get; set; }

        public string Message { get; set; }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(Email))
            {
                return RedirectToPage("Register");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            var json = HttpContext.Session.GetString("RegisterRequestJson");
            if (!string.IsNullOrEmpty(json))
            {
                RegisterRequest = JsonSerializer.Deserialize<RegisterRequest>(json);
            }

            if (RegisterModel.otpMemory.TryGetValue(Email, out var store))
            {
                if (store.Otp == OtpInput && store.ExpirationTime > DateTime.Now)
                {
                    var client = _clientFactory.CreateClient("API");
                    var jsons = JsonSerializer.Serialize(RegisterRequest);
                    var content = new StringContent(jsons, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("/api/Auth/Register", content);

                    if (response.IsSuccessStatusCode)
                    {
                        RegisterModel.otpMemory.TryRemove(Email, out _);
                        TempData["RegisterSuccess"] = true;
                        return RedirectToPage(); // redirect lại chính trang để hiển thị popup
                    }


                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, error);
                }
                else
                {
                    Message = "Mã OTP không chính xác hoặc đã hết hạn.";
                }
            }
            else
            {
                Message = "Không tìm thấy mã OTP. Vui lòng yêu cầu lại.";
            }

            return Page();
        }
    }
}