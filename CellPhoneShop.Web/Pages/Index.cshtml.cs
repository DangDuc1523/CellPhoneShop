using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CellPhoneShop.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<FeaturedPhone> FeaturedPhones { get; set; } = new();
        public List<PopularBrand> PopularBrands { get; set; } = new();

        public IActionResult OnGet()
        {
            // Check if user is authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                // Check user role
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                
                if (role == "Admin" || role == "Staff")
                {
                    // Redirect admin/staff to admin dashboard
                    return RedirectToPage("/Admin/Index");
                }
                else
                {
                    // Customer redirect to customer home page
                    return RedirectToPage("/Customer/Index");
                }
            }
            else
            {
                // Not authenticated, redirect to customer home page
                return RedirectToPage("/Customer/Index");
            }
        }

        private void LoadFeaturedData()
        {
            // Placeholder data - will be replaced with API calls
            FeaturedPhones = new List<FeaturedPhone>
            {
                new() { PhoneId = 1, PhoneName = "iPhone 15 Pro", BrandName = "Apple", BasePrice = 999, ImageUrl = "/images/iphone15pro.jpg" },
                new() { PhoneId = 2, PhoneName = "Samsung Galaxy S24", BrandName = "Samsung", BasePrice = 899, ImageUrl = "/images/galaxys24.jpg" },
                new() { PhoneId = 3, PhoneName = "Google Pixel 8", BrandName = "Google", BasePrice = 699, ImageUrl = "/images/pixel8.jpg" },
                new() { PhoneId = 4, PhoneName = "OnePlus 12", BrandName = "OnePlus", BasePrice = 799, ImageUrl = "/images/oneplus12.jpg" }
            };

            PopularBrands = new List<PopularBrand>
            {
                new() { BrandId = 1, BrandName = "Apple" },
                new() { BrandId = 2, BrandName = "Samsung" },
                new() { BrandId = 3, BrandName = "Google" },
                new() { BrandId = 4, BrandName = "OnePlus" },
                new() { BrandId = 5, BrandName = "Xiaomi" },
                new() { BrandId = 6, BrandName = "Huawei" }
            };
        }

        public class FeaturedPhone
        {
            public int PhoneId { get; set; }
            public string PhoneName { get; set; } = string.Empty;
            public string BrandName { get; set; } = string.Empty;
            public decimal BasePrice { get; set; }
            public string ImageUrl { get; set; } = string.Empty;
        }

        public class PopularBrand
        {
            public int BrandId { get; set; }
            public string BrandName { get; set; } = string.Empty;
        }
    }
}
