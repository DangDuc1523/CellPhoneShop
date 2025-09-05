using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using CellPhoneShop.Web.DTOs.Attribute;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Admin.VariantAttribute;

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
    public UpdateVariantAttributeDto VariantAttribute { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/VariantAttribute/{id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var variantAttribute = JsonSerializer.Deserialize<VariantAttributeDto>(content, options);
                
                if (variantAttribute != null)
                {
                    VariantAttribute = new UpdateVariantAttributeDto
                    {
                        VariantAttributeId = variantAttribute.VariantAttributeId,
                        Name = variantAttribute.Name,
                        Description = variantAttribute.Description,
                        Values = variantAttribute.Values ?? new List<VariantAttributeValueDto>()
                    };
                }
                else
                {
                    ErrorMessage = "Variant attribute not found.";
                }
            }
            else
            {
                ErrorMessage = "Failed to load variant attribute.";
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
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/VariantAttribute/{VariantAttribute.VariantAttributeId}";
            var json = JsonSerializer.Serialize(VariantAttribute);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Admin/VariantAttribute/Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Failed to update variant attribute. Status: {response.StatusCode}, Error: {errorContent}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return Page();
    }
} 