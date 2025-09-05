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
    public CreateBrandDto Brand { get; set; } = new();
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
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Brand";
            var json = JsonSerializer.Serialize(Brand);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Lấy token từ ClaimsPrincipal
            var token = User.FindFirst("Token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Admin/Brand/Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Failed to create brand. Status: {response.StatusCode}, Error: {errorContent}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return Page();
    }
} 