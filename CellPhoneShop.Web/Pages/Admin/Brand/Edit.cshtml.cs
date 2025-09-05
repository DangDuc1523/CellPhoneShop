using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using CellPhoneShop.Web.DTOs.Brand;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CellPhoneShop.Web.Pages.Admin.Brand;

[Authorize(Roles = "Admin,Staff")]
public class EditModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public EditModel(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    [BindProperty]
    public UpdateBrandDto Brand { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Brand/{id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var brand = JsonSerializer.Deserialize<BrandDto>(content, options);
                if (brand != null)
                {
                    Brand = new UpdateBrandDto
                    {
                        BrandId = brand.BrandId,
                        BrandName = brand.BrandName,
                        Description = brand.Description
                    };
                }
                else
                {
                    ErrorMessage = "Brand not found.";
                }
            }
            else
            {
                ErrorMessage = "Failed to load brand.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        try
        {
            var token = User.FindFirst("Token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Brand/{Brand.BrandId}";
            var json = JsonSerializer.Serialize(Brand);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Admin/Brand/Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Failed to update brand. Status: {response.StatusCode}, Error: {errorContent}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
        return Page();
    }
} 