using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using CellPhoneShop.Web.DTOs.Attribute;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Admin.VariantAttribute;

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

    public List<VariantAttributeDto> VariantAttributes { get; set; } = new();
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/VariantAttribute";
            var response = await _httpClient.GetAsync(apiUrl);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                VariantAttributes = JsonSerializer.Deserialize<List<VariantAttributeDto>>(content, options) ?? new();
            }
            else
            {
                ErrorMessage = "Failed to load variant attributes from API.";
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
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/VariantAttribute/{id}";
            var response = await _httpClient.DeleteAsync(apiUrl);
            
            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Variant attribute deleted successfully.";
            }
            else
            {
                ErrorMessage = "Failed to delete variant attribute.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return RedirectToPage();
    }
} 