using CellPhoneShop.Web.DTOs.Phone;
using CellPhoneShop.Web.DTOs.Attribute;
using CellPhoneShop.Web.DTOs.Brand;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Linq;

namespace CellPhoneShop.Web.Pages.Admin.Phone
{
    [Authorize(Roles = "Admin,Staff")]
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CreateModel> _logger;

        [BindProperty]
        public CreatePhoneDto Phone { get; set; } = new();

        public SelectList BrandList { get; set; } = null!;
        public List<PhoneAttributeDto> AvailableAttributes { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public CreateModel(IHttpClientFactory httpClientFactory, ILogger<CreateModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadBrands();
            await LoadAttributes();
            
            // Initialize AttributeMappings
            if (AvailableAttributes.Any())
            {
                Phone.AttributeMappings = AvailableAttributes.Select(attr => new PhoneAttributeMappingDto
                {
                    AttributeId = attr.AttributeId,
                    AttributeName = attr.Name,
                    Value = null
                }).ToList();
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validate and populate AttributeMappings
            if (Phone.AttributeMappings != null)
            {
                await LoadAttributes(); // Load to get attribute names
                
                for (int i = 0; i < Phone.AttributeMappings.Count; i++)
                {
                    var mapping = Phone.AttributeMappings[i];
                    var attr = AvailableAttributes.FirstOrDefault(a => a.AttributeId == mapping.AttributeId);
                    if (attr != null)
                    {
                        mapping.AttributeName = attr.Name;
                    }
                }

                // Remove empty mappings for API call
                Phone.AttributeMappings = Phone.AttributeMappings
                    .Where(x => !string.IsNullOrEmpty(x.Value))
                    .ToList();
            }

            if (!ModelState.IsValid)
            {
                await LoadBrands();
                await LoadAttributes();
                
                // Re-initialize AttributeMappings if needed
                if (Phone.AttributeMappings == null || !Phone.AttributeMappings.Any())
                {
                    Phone.AttributeMappings = AvailableAttributes.Select(attr => new PhoneAttributeMappingDto
                    {
                        AttributeId = attr.AttributeId,
                        AttributeName = attr.Name,
                        Value = null
                    }).ToList();
                }
                
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.PostAsJsonAsync("api/Phone", Phone);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Index", new { success = "Phone created successfully" });
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
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.GetAsync("api/PhoneAttribute");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var attributes = JsonSerializer.Deserialize<List<PhoneAttributeDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    AvailableAttributes = attributes ?? new();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading attributes");
                ErrorMessage = "Failed to load attributes. Some features may be limited.";
            }
        }
    }
} 