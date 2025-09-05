using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CellPhoneShop.Web.Pages.Customer;

public class IndexModel : PageModel
{
    public List<FeaturedPhone> FeaturedPhones { get; set; } = new();
    public List<PopularBrand> PopularBrands { get; set; } = new();

    public void OnGet()
    {
        LoadFeaturedData();
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