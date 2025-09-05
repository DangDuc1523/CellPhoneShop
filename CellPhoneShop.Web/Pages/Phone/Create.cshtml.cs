using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using CellPhoneShop.Web.DTOs.Phone;
using CellPhoneShop.Web.DTOs.Brand;
using CellPhoneShop.Business.DTOs.Attribute;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Phone
{
    [Authorize(Roles = "Admin,Staff")]
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IHttpClientFactory httpClientFactory, ILogger<CreateModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [BindProperty]
        public CreatePhoneDto Phone { get; set; } = new();
        
        public SelectList BrandList { get; set; } = null!;
        public List<AvailableAttribute> AvailableAttributes { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadBrands();
            await LoadAttributes();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadBrands();
                await LoadAttributes();
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient("API");

                // Add JWT token to request header
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.PostAsJsonAsync("api/Phone", Phone);
                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Admin/Phone/Index", new { success = "Phone created successfully" });
                }
                
                ErrorMessage = "Failed to create phone. Please try again.";
                await LoadBrands();
                await LoadAttributes();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating phone");
                ErrorMessage = "An unexpected error occurred. Please try again later.";
                await LoadBrands();
                await LoadAttributes();
                return Page();
            }
        }

        private async Task LoadBrands()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                // Add JWT token to request header
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.GetAsync("api/Brand");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var brands = JsonSerializer.Deserialize<List<BrandDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    BrandList = new SelectList(brands, "BrandId", "BrandName");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading brands");
                ErrorMessage = "Failed to load brands. Some features may be limited.";
            }
        }

        private async Task LoadAttributes()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                // Add JWT token to request header
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.GetAsync("api/PhoneAttribute");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var attributes = JsonSerializer.Deserialize<List<PhoneAttributeDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    AvailableAttributes = attributes?.Select(a => new AvailableAttribute
                    {
                        AttributeId = a.AttributeId,
                        Name = a.Name
                    }).ToList() ?? new();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading attributes");
                ErrorMessage = "Failed to load attributes. Some features may be limited.";
            }
        }

        public class AvailableAttribute
        {
            public int AttributeId { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
} 