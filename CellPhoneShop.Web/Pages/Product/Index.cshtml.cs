using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Product
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public List<PhoneListItemDto> Phones { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var response = await client.GetAsync("api/Phone");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var phones = JsonSerializer.Deserialize<List<PhoneListItemDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (phones != null)
                    {
                        Phones = phones;
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách điện thoại");
                return Page();
            }
        }
    }

    public class PhoneListItemDto
    {
        public int PhoneId { get; set; }
        public string PhoneName { get; set; } = "";
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Ram { get; set; }
        public string? Storage { get; set; }
    }
}
