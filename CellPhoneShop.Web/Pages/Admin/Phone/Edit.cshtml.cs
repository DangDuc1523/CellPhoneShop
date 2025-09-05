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

namespace CellPhoneShop.Web.Pages.Admin.Phone
{
    [Authorize(Roles = "Admin,Staff")]
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EditModel> _logger;

        [BindProperty]
        public UpdatePhoneDto Phone { get; set; } = new();

        public SelectList BrandList { get; set; } = null!;
        public List<PhoneAttributeDto> AvailableAttributes { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public EditModel(IHttpClientFactory httpClientFactory, ILogger<EditModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                _logger.LogInformation("Fetching phone with ID: {PhoneId}", id);
                var response = await client.GetAsync($"api/Phone/{id}");
                
                _logger.LogInformation("API Response Status: {StatusCode}", response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("API Response Content: {Content}", content);
                    
                    var phone = JsonSerializer.Deserialize<UpdatePhoneDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (phone != null)
                    {
                        _logger.LogInformation("Phone deserialized successfully. Phone: {@Phone}", phone);
                        Phone = phone;
                        await LoadBrands();
                        await LoadAttributes();
                        
                        _logger.LogInformation("Available attributes count: {Count}", AvailableAttributes.Count);
                        
                        // Initialize AttributeMappings properly
                        if (Phone.AttributeMappings == null)
                            Phone.AttributeMappings = new List<PhoneAttributeMappingDto>();

                        // Create a complete list with all attributes
                        var newMappings = new List<PhoneAttributeMappingDto>();
                        foreach (var attr in AvailableAttributes)
                        {
                            var existingMapping = Phone.AttributeMappings.FirstOrDefault(x => x.AttributeId == attr.AttributeId);
                            newMappings.Add(new PhoneAttributeMappingDto
                            {
                                AttributeId = attr.AttributeId,
                                AttributeName = attr.Name,
                                Value = existingMapping?.Value
                            });
                        }
                        Phone.AttributeMappings = newMappings;
                        
                        _logger.LogInformation("Final AttributeMappings count: {Count}", Phone.AttributeMappings.Count);
                        return Page();
                    }
                    else
                    {
                        _logger.LogWarning("Phone deserialization returned null");
                    }
                }
                else
                {
                    _logger.LogWarning("API call failed with status: {StatusCode}", response.StatusCode);
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Error content: {ErrorContent}", errorContent);
                }

                ErrorMessage = "Phone not found.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading phone for edit with ID: {PhoneId}", id);
                ErrorMessage = "An error occurred while loading the phone.";
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync called for Phone ID: {PhoneId}", Phone.PhoneId);
            _logger.LogInformation("ModelState.IsValid: {IsValid}", ModelState.IsValid);
            
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
            }
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors: {@Errors}", 
                    ModelState.Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)));
                        
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
                
                _logger.LogInformation("Token from claims: {Token}", token?.Substring(0, Math.Min(20, token?.Length ?? 0)) + "...");
                _logger.LogInformation("User Identity Name: {Name}", User.Identity?.Name);
                _logger.LogInformation("User IsAuthenticated: {IsAuth}", User.Identity?.IsAuthenticated);
                _logger.LogInformation("User Claims: {@Claims}", User.Claims.Select(c => new { c.Type, c.Value }).ToList());
                
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Token is null or empty!");
                    ErrorMessage = "Authentication token is missing. Please login again.";
                    await LoadBrands();
                    await LoadAttributes();
                    return Page();
                }
                
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    _logger.LogInformation("Authorization header set with Bearer token");
                }

                // Validate required fields
                if (!Phone.BrandId.HasValue || Phone.BrandId <= 0)
                {
                    _logger.LogError("Invalid BrandId: {BrandId}", Phone.BrandId);
                    ErrorMessage = "Brand is required.";
                    await LoadBrands();
                    await LoadAttributes();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(Phone.PhoneName))
                {
                    _logger.LogError("PhoneName is null or empty");
                    ErrorMessage = "Phone name is required.";
                    await LoadBrands();
                    await LoadAttributes();
                    return Page();
                }

                if (Phone.BasePrice <= 0)
                {
                    _logger.LogError("Invalid BasePrice: {BasePrice}", Phone.BasePrice);
                    ErrorMessage = "Base price must be greater than 0.";
                    await LoadBrands();
                    await LoadAttributes();
                    return Page();
                }

                // Create API DTO to match Business.DTOs.UpdatePhoneDto format exactly
                var filteredMappings = Phone.AttributeMappings?.Where(x => !string.IsNullOrEmpty(x.Value)) ?? new List<PhoneAttributeMappingDto>();
                var attributeMappings = filteredMappings.Select(x => new 
                { 
                    AttributeId = x.AttributeId, 
                    Value = x.Value
                    // Remove AttributeName - Business DTO doesn't need it
                }).ToList();

                _logger.LogInformation("AttributeMappings before filtering: {@AllMappings}", Phone.AttributeMappings?.Select(x => new { x.AttributeId, x.Value, x.AttributeName }).ToList());
                _logger.LogInformation("AttributeMappings after filtering: {@FilteredMappings}", attributeMappings);

                // Send exactly what Business.DTOs.UpdatePhoneDto expects
                var businessUpdateDto = new
                {
                    BrandId = Phone.BrandId.Value, // Ensure not null
                    PhoneName = Phone.PhoneName.Trim(), // Trim whitespace
                    Description = Phone.Description?.Trim(), // Handle null
                    BasePrice = Phone.BasePrice,
                    AttributeMappings = attributeMappings
                };

                _logger.LogInformation("Sending update request for Phone ID: {PhoneId}", Phone.PhoneId);
                _logger.LogInformation("Business Update DTO: {@UpdateDto}", businessUpdateDto);
                _logger.LogInformation("API URL: {Url}", $"api/Phone/{Phone.PhoneId}");

                var response = await client.PutAsJsonAsync($"api/Phone/{Phone.PhoneId}", businessUpdateDto);
                
                _logger.LogInformation("Update response status: {StatusCode}", response.StatusCode);
                _logger.LogInformation("Response headers: {@Headers}", response.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value)));

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Phone updated successfully");
                    return RedirectToPage("Index", new { success = "Phone updated successfully" });
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Update failed. Status: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
                _logger.LogError("Request URL: {Url}", $"api/Phone/{Phone.PhoneId}");
                _logger.LogError("Request Body: {@Body}", businessUpdateDto);
                
                // Show detailed error in UI for debugging
                ErrorMessage = $"Failed to update phone. Status: {response.StatusCode}. Details: {errorContent}";
                await LoadBrands();
                await LoadAttributes();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating phone with ID: {PhoneId}", Phone.PhoneId);
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