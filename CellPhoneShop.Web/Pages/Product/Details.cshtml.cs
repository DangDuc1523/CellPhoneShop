using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Product
{
    [AllowAnonymous]
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DetailsModel> _logger;
        public DetailsModel(IHttpClientFactory httpClientFactory, ILogger<DetailsModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public PhoneDetailDto? Phone { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var response = await client.GetAsync($"api/Phone/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Phone = JsonSerializer.Deserialize<PhoneDetailDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (Phone == null)
                        return NotFound();
                }
                else
                {
                    return NotFound();
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết sản phẩm");
                return NotFound();
            }
        }
        public class PhoneDetailDto
        {
            public int PhoneId { get; set; }
            public string PhoneName { get; set; }
            public string? BrandName { get; set; }
            public string? Color { get; set; }
            public string? ImageUrl { get; set; }
            public string? Description { get; set; }
            public string? Screen { get; set; }
            public string? Ram { get; set; }
            public string? Storage { get; set; }
            public string? Battery { get; set; }
            public decimal? Price { get; set; }
        }
    }
}
