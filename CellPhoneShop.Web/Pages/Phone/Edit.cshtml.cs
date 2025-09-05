using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using CellPhoneShop.Web.DTOs.Phone;
using CellPhoneShop.Web.DTOs.Brand;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Phone
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IHttpClientFactory httpClientFactory, ILogger<EditModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [BindProperty]
        public UpdatePhoneDto Phone { get; set; } = new();
        
        public SelectList BrandList { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
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

                var response = await client.GetAsync($"api/Phone/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var phoneDto = JsonSerializer.Deserialize<PhoneDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (phoneDto != null)
                    {
                        Phone = new UpdatePhoneDto
                        {
                            PhoneId = phoneDto.PhoneId,
                            BrandId = phoneDto.BrandId,
                            PhoneName = phoneDto.PhoneName,
                            Description = phoneDto.Description,
                            BasePrice = phoneDto.BasePrice,
                            Screen = phoneDto.Screen,
                            Os = phoneDto.Os,
                            FrontCamera = phoneDto.FrontCamera,
                            RearCamera = phoneDto.RearCamera,
                            Cpu = phoneDto.Cpu,
                            Ram = phoneDto.Ram,
                            Battery = phoneDto.Battery,
                            Sim = phoneDto.Sim,
                            Other = phoneDto.Other
                        };
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }

                await LoadBrands();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading phone");
                ErrorMessage = "An unexpected error occurred. Please try again later.";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadBrands();
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

                var response = await client.PutAsJsonAsync($"api/Phone/{Phone.PhoneId}", Phone);
                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index", new { success = "Phone updated successfully" });
                }
                
                ErrorMessage = "Failed to update phone. Please try again.";
                await LoadBrands();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating phone");
                ErrorMessage = "An unexpected error occurred. Please try again later.";
                await LoadBrands();
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
    }
} 