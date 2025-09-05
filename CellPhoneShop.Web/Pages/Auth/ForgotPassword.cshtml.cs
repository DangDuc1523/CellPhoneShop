using CellPhoneShop.Web.DTOs.Account;
using CellPhoneShop.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace CellPhoneShop.Web.Pages.Auth
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IEmailService _emailService;
        public static ConcurrentDictionary<string, OtpStore> otpMemory = new();

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        public string Message { get; set; }

        public ForgotPasswordModel(IHttpClientFactory clientFactory,IEmailService emailService)
        {
            _emailService = emailService;
            _clientFactory = clientFactory;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
   
            var httpClient = _clientFactory.CreateClient("API");
            var response = await httpClient.PostAsJsonAsync("/api/Auth/check-email", Email);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Không thể kiểm tra email. Vui lòng thử lại.");
                return Page();
            }

            var exists = await response.Content.ReadFromJsonAsync<bool>();
            if (!exists)
            {
                ModelState.AddModelError(string.Empty, "Email không tồn tại trong hệ thống.");
                return Page();
            }

            var otp = new Random().Next(100000, 999999).ToString();
            var store = new OtpStore
            {
                Email = Email,
                Otp = otp,
                ExpirationTime = DateTime.Now.AddMinutes(5),

            };

            otpMemory[Email] = store;

            await _emailService.SendOtpEmailAsync(Email, $"Bạn đang yêu cầu cấp lại mật khẩu. Mã OTP của bạn là {otp}");

            Message = "Mã OTP đã được gửi đến email của bạn.";
            return RedirectToPage("VerifyOtp", new { email = Email });
        }
    }
}
