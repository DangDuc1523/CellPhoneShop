using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CellPhoneShop.Web.Pages.Auth
{
    public class VerifyOtpModel : PageModel
    {
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
                return RedirectToPage("ForgotPassword");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (ForgotPasswordModel.otpMemory.TryGetValue(Email, out var store))
            {
                if (store.Otp == OtpInput && store.ExpirationTime > DateTime.Now)
                {
                    return RedirectToPage("ResetPassword", new { email = Email });
                }
                else
                {
                    Message = "Mã OTP không chính xác hoặc đã hết hạn.";
                }
            }
            else
            {
                Message = "Không tìm thấy mã OTP. Vui lòng yêu cầu lại...!";
            }

            return Page();
        }
    }
}
