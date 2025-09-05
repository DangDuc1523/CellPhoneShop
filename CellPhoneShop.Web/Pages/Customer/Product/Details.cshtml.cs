using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using CellPhoneShop.Web.DTOs.Phone;

namespace CellPhoneShop.Web.Pages.Customer.Product
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IHttpClientFactory httpClientFactory, ILogger<DetailsModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public PhoneDto? Phone { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Hardcoded data for demonstration
            Phone = new PhoneDto
            {
                PhoneId = id,
                PhoneName = "Điện Thoại Mẫu " + id,
                BasePrice = 15000000 + (id * 100000), // Example price
                BrandName = "Thương Hiệu Mẫu",
                Description = "Đây là mô tả mẫu cho điện thoại. Bạn có thể thêm nhiều thông tin hơn ở đây để kiểm tra bố cục giao diện.",
                Screen = "6.1 inch OLED",
                Os = "Android 14",
                Cpu = "Snapdragon 8 Gen 2",
                Ram = "8GB",
                Battery = "4500mAh",
                FrontCamera = "12MP",
                RearCamera = "50MP Triple Camera",
                Sim = "Dual SIM",
                Other = "Chống nước, Sạc nhanh"
            };

            return Page();
            /*
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var response = await client.GetAsync($"api/Phone/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Phone = JsonSerializer.Deserialize<PhoneDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (Phone == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    // Log the API response status and content for debugging
                    _logger.LogError("API call to get phone details failed with status code: {StatusCode}. Content: {Content}", response.StatusCode, await response.Content.ReadAsStringAsync());
                    return NotFound();
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading phone details from API for ID: {Id}", id);
                ErrorMessage = "An unexpected error occurred. Please try again later.";
                return RedirectToPage("./Product"); // Redirect back to the product list page
            }
            */
        }
    }
} 