using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using CellPhoneShop.Web.DTOs.Attribute;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Admin.Attribute;

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
    public UpdateAttributeDto Attribute { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Attribute/{id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var attribute = JsonSerializer.Deserialize<AttributeDto>(content, options);
                
                if (attribute != null)
                {
                    Attribute = new UpdateAttributeDto
                    {
                        AttributeId = attribute.AttributeId,
                        Name = attribute.Name,
                        Description = attribute.Description
                    };
                }
                else
                {
                    ErrorMessage = "Attribute not found.";
                }
            }
            else
            {
                ErrorMessage = "Failed to load attribute.";
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
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Attribute/{Attribute.AttributeId}";
            var json = JsonSerializer.Serialize(Attribute);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Admin/Attribute/Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Failed to update attribute. Status: {response.StatusCode}, Error: {errorContent}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return Page();
    }
} 