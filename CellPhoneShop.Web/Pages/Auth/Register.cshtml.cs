using Azure;
using CellPhoneShop.Web.DTOs.Account;
using CellPhoneShop.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CellPhoneShop.Web.Pages.Auth
{
    //DANGDUC
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public static ConcurrentDictionary<string, OtpStore> otpMemory = new();

        [BindProperty]
        public RegisterRequest RegisterRequest { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public string Message { get; set; }

        public RegisterModel(IHttpClientFactory clientFactory, IConfiguration configuration, IEmailService emailService)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _emailService = emailService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra dữ liệu hợp lệ từ form
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = _clientFactory.CreateClient("API");

            var response = await httpClient.PostAsJsonAsync("/api/Auth/check-email", RegisterRequest.Email);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Không thể kiểm tra email. Vui lòng thử lại.");
                return Page();
            }

            var exists = await response.Content.ReadFromJsonAsync<bool>();
            if (exists)
            {
                ModelState.AddModelError(string.Empty, "Email đã tồn tại trong hệ thống.");
                return Page();
            }

            var otp = new Random().Next(100000, 999999).ToString();
            var store = new OtpStore
            {
                Email = RegisterRequest.Email,
                Otp = otp,
                ExpirationTime = DateTime.Now.AddMinutes(5)
            };

            otpMemory[RegisterRequest.Email] = store;

            await _emailService.SendOtpEmailAsync(RegisterRequest.Email, $"Bạn đang đăng ký tài khoản mới. Mã OTP của bạn là {otp}");

            var registerJson = JsonSerializer.Serialize(RegisterRequest);
            HttpContext.Session.SetString("RegisterRequestJson", registerJson);

            Message = "Mã OTP đã được gửi đến email của bạn.";

            return RedirectToPage("VerifyOtpRegister", new { email = RegisterRequest.Email });
        }
        //DANGDUC
    }

    public class RegisterRequest
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
