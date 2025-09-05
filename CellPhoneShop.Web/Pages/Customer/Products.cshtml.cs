using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CellPhoneShop.Web.Pages.Customer
{
    public class ProductsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductsModel> _logger;

        public ProductsModel(IHttpClientFactory httpClientFactory, ILogger<ProductsModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public List<BrandDto> Brands { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await LoadBrandsAsync();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products page");
                return Page();
            }
        }

        private async Task LoadBrandsAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var response = await httpClient.GetAsync("api/brand");
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Brands = JsonSerializer.Deserialize<List<BrandDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading brands");
                Brands = new();
            }
        }
    }

    // DTO for Brand
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
} 