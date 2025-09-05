using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using CellPhoneShop.Web.DTOs.Brand;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Admin.Brand;

[Authorize(Roles = "Admin,Staff")]
public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public IndexModel(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public List<BrandDto> Brands { get; set; } = new();
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Brand";
            var response = await _httpClient.GetAsync(apiUrl);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Brands = JsonSerializer.Deserialize<List<BrandDto>>(content, options) ?? new();
            }
            else
            {
                ErrorMessage = "Failed to load brands from API.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Brand/{id}";
            var response = await _httpClient.DeleteAsync(apiUrl);
            
            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Brand deleted successfully.";
            }
            else
            {
                ErrorMessage = "Failed to delete brand.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return RedirectToPage();
    }
} 