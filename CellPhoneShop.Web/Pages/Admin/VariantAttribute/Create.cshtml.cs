using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using CellPhoneShop.Web.DTOs.Attribute;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Admin.VariantAttribute;

[Authorize(Roles = "Admin,Staff")]
public class CreateModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CreateModel(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    [BindProperty]
    public CreateVariantAttributeDto VariantAttribute { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/VariantAttribute";
            var json = JsonSerializer.Serialize(VariantAttribute);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Admin/VariantAttribute/Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Failed to create variant attribute. Status: {response.StatusCode}, Error: {errorContent}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return Page();
    }
} 