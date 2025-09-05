using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CellPhoneShop.Web.Services;
using System.Threading.Tasks;

namespace CellPhoneShop.Web.Pages.Customer.Home
{
    public class HomeModel : PageModel
    {
        private readonly EmailService _emailService;
        public HomeModel(EmailService emailService)
        {
            _emailService = emailService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSubscribeAsync(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                await _emailService.SendThankYouEmailAsync(email);
                TempData["SubscribeMessage"] = "Cảm ơn bạn đã đăng ký! Vui lòng kiểm tra email.";
            }
            return RedirectToPage();
        }
    }
}
