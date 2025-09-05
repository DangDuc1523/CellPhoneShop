using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CellPhoneShop.Web.DTOs.Phone;
using CellPhoneShop.Web.DTOs.Color;
using CellPhoneShop.Web.DTOs.Variant;
using AttributeDTOs = CellPhoneShop.Web.DTOs.Attribute;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Security.Claims;

namespace CellPhoneShop.Web.Pages.Admin.Phone
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<DetailsModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        [BindProperty]
        public PhoneDto Phone { get; set; } = new();

        public List<ColorDto> Colors { get; set; } = new();
        public List<PhoneVariantDto> PhoneVariants { get; set; } = new();
        public List<VariantAttributeDto> VariantAttributes { get; set; } = new();

        [TempData]
        public string SuccessMessage { get; set; } = string.Empty;

        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;

        // Add Color form properties
        [BindProperty]
        public int PhoneId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Color name is required")]
        public string ColorName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string ImageUrl { get; set; } = string.Empty;

        // Generate Variants form properties
        [BindProperty]
        public List<int> SelectedColors { get; set; } = new();

        [BindProperty]
        public List<int> SelectedAttributeValues { get; set; } = new();

        [BindProperty]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal BasePrice { get; set; }

        [BindProperty]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0")]
        public int BaseStock { get; set; } = 100;

        // Edit Variant form properties
        [BindProperty]
        public int EditVariantId { get; set; }

        [BindProperty]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal EditPrice { get; set; }

        [BindProperty]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0")]
        public int EditStock { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _logger.LogInformation("=== OnGetAsync called with id: {Id} ===", id);
            
            try
            {
                await LoadDataAsync(id);
                
                // Debug Phone loading
                _logger.LogInformation("After LoadDataAsync - Phone object: {@Phone}", Phone);
                _logger.LogInformation("Phone.PhoneId: {PhoneId}", Phone?.PhoneId);
                _logger.LogInformation("PhoneId property: {PhoneIdProperty}", PhoneId);
                
                return Page();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogError("Phone not found with id: {Id}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading phone details for id: {Id}", id);
                ErrorMessage = $"Error loading phone details: {ex.Message}";
                return RedirectToPage("/Admin/Phone/Index");
            }
        }

        public async Task<IActionResult> OnPostAddColorAsync()
        {
            _logger.LogInformation("=== Starting OnPostAddColorAsync ===");
            _logger.LogInformation("PhoneId: {PhoneId}, ColorName: {ColorName}, ImageUrl: {ImageUrl}", PhoneId, ColorName, ImageUrl);
            
            // Clear validation errors for fields not used in AddColor
            ModelState.Remove(nameof(BasePrice));
            ModelState.Remove(nameof(BaseStock));
            ModelState.Remove(nameof(SelectedColors));
            ModelState.Remove(nameof(SelectedAttributeValues));
            
            // Manual validation for AddColor specific fields
            if (string.IsNullOrWhiteSpace(ColorName))
            {
                ModelState.AddModelError(nameof(ColorName), "Color name is required");
            }
            
            if (string.IsNullOrWhiteSpace(ImageUrl))
            {
                ModelState.AddModelError(nameof(ImageUrl), "Image URL is required");
            }
            
            if (PhoneId <= 0)
            {
                ModelState.AddModelError(nameof(PhoneId), "Valid Phone ID is required");
            }
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid for AddColor");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Validation error: {Error}", error.ErrorMessage);
                }
                await LoadDataAsync(PhoneId);
                return Page();
            }

            try
            {
                // Get PhoneId from route parameter
                var routePhoneId = RouteData.Values["id"]?.ToString();
                if (int.TryParse(routePhoneId, out int parsedPhoneId))
                {
                    PhoneId = parsedPhoneId;
                }
                else
                {
                    _logger.LogError("Invalid PhoneId in route: {RouteId}", routePhoneId);
                    ErrorMessage = "Invalid Phone ID in URL";
                    return RedirectToPage("/Admin/Phone/Index");
                }
                
                // Create DTO compatible with Business layer
                var createColorDto = new CreateColorDto
                {
                    PhoneId = PhoneId,
                    ColorName = ColorName,
                    ImageUrl = ImageUrl
                };

                // Debug PhoneID value
                _logger.LogInformation("=== DEBUGGING PHONE ID ===");
                _logger.LogInformation("PhoneId from form: {PhoneId}", PhoneId);
                _logger.LogInformation("Phone.PhoneId from model: {ModelPhoneId}", Phone?.PhoneId);
                _logger.LogInformation("URL parameter id: {UrlId}", RouteData.Values["id"]);
                
                // Verify phone exists by trying to get it first
                var httpClient = _httpClientFactory.CreateClient("API");
                
                // Get token from claims with detailed logging
                var token = GetTokenFromClaims();
                _logger.LogInformation("Token retrieved from claims: {TokenExists}", !string.IsNullOrEmpty(token));
                _logger.LogInformation("User Identity Name: {Name}", User.Identity?.Name);
                _logger.LogInformation("User IsAuthenticated: {IsAuth}", User.Identity?.IsAuthenticated);
                _logger.LogInformation("User Claims Count: {ClaimsCount}", User.Claims.Count());
                
                foreach (var claim in User.Claims)
                {
                    _logger.LogInformation("Claim - Type: {Type}, Value: {Value}", claim.Type, 
                        claim.Type == "Token" ? (claim.Value?.Length > 20 ? claim.Value[..20] + "..." : claim.Value) : claim.Value);
                }
                
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Token is null or empty! User needs to login again.");
                    ErrorMessage = "Authentication token is missing. Please login again.";
                    await LoadDataAsync(PhoneId);
                    return Page();
                }

                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation("Authorization header set with Bearer token");

                // First, verify the phone exists
                _logger.LogInformation("Verifying phone exists with ID: {PhoneId}", PhoneId);
                var phoneCheckResponse = await httpClient.GetAsync($"api/Phone/{PhoneId}");
                _logger.LogInformation("Phone verification response: {StatusCode}", phoneCheckResponse.StatusCode);
                
                if (!phoneCheckResponse.IsSuccessStatusCode)
                {
                    var phoneErrorContent = await phoneCheckResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Phone does not exist! PhoneId: {PhoneId}, Error: {Error}", PhoneId, phoneErrorContent);
                    ErrorMessage = $"Phone with ID {PhoneId} does not exist. Cannot add color.";
                    await LoadDataAsync(PhoneId);
                    return Page();
                }

                var json = JsonSerializer.Serialize(createColorDto, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                // Log detailed request information
                _logger.LogInformation("=== API REQUEST DETAILS ===");
                _logger.LogInformation("Base URL: {BaseUrl}", httpClient.BaseAddress?.ToString());
                _logger.LogInformation("Endpoint: api/Color");
                _logger.LogInformation("Full URL: {FullUrl}", $"{httpClient.BaseAddress}api/Color");
                _logger.LogInformation("HTTP Method: POST");
                _logger.LogInformation("Content-Type: application/json");
                _logger.LogInformation("Authorization: Bearer {TokenPrefix}...", token?.Length > 10 ? token[..10] : token);
                _logger.LogInformation("Request Body: {RequestBody}", json);
                _logger.LogInformation("Request Headers: {Headers}", string.Join(", ", httpClient.DefaultRequestHeaders.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}")));
                
                _logger.LogInformation("Sending POST request to api/Color with data: {Data}", json);

                var response = await httpClient.PostAsync("api/Color", content);
                
                // Log detailed response information
                _logger.LogInformation("=== API RESPONSE DETAILS ===");
                _logger.LogInformation("API Response Status: {StatusCode}", response.StatusCode);
                _logger.LogInformation("Response Headers: {Headers}", string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}")));
                
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Response Content: {Content}", responseContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Color added successfully");
                    SuccessMessage = $"Color '{ColorName}' has been added successfully!";
                    return RedirectToPage(new { id = PhoneId });
                }
                else
                {
                    _logger.LogError("API Error - Status: {StatusCode}, Content: {Content}", response.StatusCode, responseContent);
                    ErrorMessage = $"Failed to add color. Status: {response.StatusCode}, Error: {responseContent}";
                    await LoadDataAsync(PhoneId);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in OnPostAddColorAsync");
                ErrorMessage = $"Error adding color: {ex.Message}";
                await LoadDataAsync(PhoneId);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteColorAsync(int colorId)
        {
            _logger.LogInformation("=== Starting OnPostDeleteColorAsync ===");
            _logger.LogInformation("ColorId: {ColorId}, PhoneId: {PhoneId}", colorId, PhoneId);
            
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                
                // Get token from claims with detailed logging
                var token = GetTokenFromClaims();
                _logger.LogInformation("Token retrieved for delete: {TokenExists}", !string.IsNullOrEmpty(token));
                
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Token is null or empty for delete operation!");
                    ErrorMessage = "Authentication token is missing. Please login again.";
                    return RedirectToPage(new { id = PhoneId });
                }

                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation("Authorization header set for delete operation");

                _logger.LogInformation("Sending DELETE request to api/Color/{ColorId}", colorId);
                var response = await httpClient.DeleteAsync($"api/Color/{colorId}");
                _logger.LogInformation("Delete API Response Status: {StatusCode}", response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Color deleted successfully");
                    SuccessMessage = "Color deleted successfully!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Delete API Error - Status: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
                    ErrorMessage = "Failed to delete color. Please try again.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in OnPostDeleteColorAsync");
                ErrorMessage = $"Error deleting color: {ex.Message}";
            }

            return RedirectToPage(new { id = PhoneId });
        }

        public async Task<IActionResult> OnPostGenerateVariantsAsync()
        {
            _logger.LogInformation("=== Starting Variant Generation ===");
            _logger.LogInformation("PhoneId: {PhoneId}, Colors: {ColorCount}, Attributes: {AttributeCount}", 
                PhoneId, SelectedColors?.Count ?? 0, SelectedAttributeValues?.Count ?? 0);
            
            // Clear validation errors for fields not used in GenerateVariants
            ModelState.Remove(nameof(ColorName));
            ModelState.Remove(nameof(ImageUrl));
            
            // Manual validation for GenerateVariants specific fields
            if (PhoneId <= 0)
            {
                ModelState.AddModelError(nameof(PhoneId), "Valid Phone ID is required");
            }
            
            if (BasePrice <= 0)
            {
                ModelState.AddModelError(nameof(BasePrice), "Base price must be greater than 0");
            }
            
            if (BaseStock < 0)
            {
                ModelState.AddModelError(nameof(BaseStock), "Base stock cannot be negative");
            }
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation failed for GenerateVariants");
                await LoadDataAsync(PhoneId);
                return Page();
            }

            // Get PhoneId from route parameter if not set
            if (PhoneId <= 0)
            {
                var routePhoneId = RouteData.Values["id"]?.ToString();
                if (int.TryParse(routePhoneId, out int parsedPhoneId))
                {
                    PhoneId = parsedPhoneId;
                }
                else
                {
                    _logger.LogError("Invalid PhoneId in route: {RouteId}", routePhoneId);
                    ErrorMessage = "Invalid Phone ID in URL";
                    return RedirectToPage("/Admin/Phone/Index");
                }
            }

            if (!SelectedColors.Any())
            {
                ErrorMessage = "Please select at least one color.";
                await LoadDataAsync(PhoneId);
                return Page();
            }

            // 🔧 FIX: Reload VariantAttributes if they're missing (lost between OnGet and OnPost)
            if (VariantAttributes == null || !VariantAttributes.Any())
            {
                _logger.LogWarning("VariantAttributes missing, reloading...");
                await ReloadVariantAttributesAsync();
                _logger.LogInformation("Reloaded {Count} attributes with {ValueCount} total values", 
                    VariantAttributes.Count, VariantAttributes.Sum(a => a.Values?.Count ?? 0));
            }

            try
            {
                await GenerateVariantsFromCombinations();
                SuccessMessage = "Variants have been generated successfully!";
                return RedirectToPage(new { id = PhoneId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating variants: {Message}", ex.Message);
                ErrorMessage = $"Error generating variants: {ex.Message}";
                await LoadDataAsync(PhoneId);
                return Page();
            }
        }

        private async Task ReloadVariantAttributesAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("API");
            var token = GetTokenFromClaims();
            
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                var attributesResponse = await httpClient.GetAsync("api/VariantAttribute/with-values");
                
                if (attributesResponse.IsSuccessStatusCode)
                {
                    var attributesJson = await attributesResponse.Content.ReadAsStringAsync();
                    VariantAttributes = JsonSerializer.Deserialize<List<VariantAttributeDto>>(attributesJson, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<VariantAttributeDto>();
                    
                    _logger.LogInformation("Reloaded {Count} attributes with {ValueCount} total values", 
                        VariantAttributes.Count, VariantAttributes.Sum(a => a.Values?.Count ?? 0));
                }
                else
                {
                    _logger.LogError("Failed to reload VariantAttributes - Status: {StatusCode}", attributesResponse.StatusCode);
                    VariantAttributes = new List<VariantAttributeDto>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while reloading VariantAttributes");
                VariantAttributes = new List<VariantAttributeDto>();
            }
        }

        private async Task LoadDataAsync(int phoneId)
        {
            _logger.LogInformation("Loading data for PhoneId: {PhoneId}", phoneId);
            
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var token = GetTokenFromClaims();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                // Load Phone data
                var phoneResponse = await httpClient.GetAsync($"api/Phone/{phoneId}");
                if (phoneResponse.IsSuccessStatusCode)
                {
                    var phoneJson = await phoneResponse.Content.ReadAsStringAsync();
                    Phone = JsonSerializer.Deserialize<PhoneDto>(phoneJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    _logger.LogError("Failed to load phone data - Status: {StatusCode}", phoneResponse.StatusCode);
                }

                // Load Colors
                var colorsResponse = await httpClient.GetAsync($"api/Color/phone/{phoneId}");
                if (colorsResponse.IsSuccessStatusCode)
                {
                    var colorsJson = await colorsResponse.Content.ReadAsStringAsync();
                    Colors = JsonSerializer.Deserialize<List<ColorDto>>(colorsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ColorDto>();
                    _logger.LogInformation("Loaded {Count} colors", Colors.Count);
                }
                else
                {
                    Colors = new List<ColorDto>();
                    _logger.LogError("Failed to load colors - Status: {StatusCode}", colorsResponse.StatusCode);
                }

                // Load Phone Variants
                var variantsResponse = await httpClient.GetAsync($"api/PhoneVariant/phone/{phoneId}");
                if (variantsResponse.IsSuccessStatusCode)
                {
                    var variantsJson = await variantsResponse.Content.ReadAsStringAsync();
                    PhoneVariants = JsonSerializer.Deserialize<List<PhoneVariantDto>>(variantsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<PhoneVariantDto>();
                    _logger.LogInformation("Loaded {Count} variants", PhoneVariants.Count);
                    
                    // Load variant attributes for each variant
                    foreach (var variant in PhoneVariants)
                    {
                        var variantAttributesResponse = await httpClient.GetAsync($"api/PhoneVariant/{variant.VariantId}/attributes");
                        if (variantAttributesResponse.IsSuccessStatusCode)
                        {
                            var variantAttributesJson = await variantAttributesResponse.Content.ReadAsStringAsync();
                            variant.VariantAttributes = JsonSerializer.Deserialize<List<VariantAttributeMappingDto>>(variantAttributesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<VariantAttributeMappingDto>();
                        }
                    }
                }
                else
                {
                    PhoneVariants = new List<PhoneVariantDto>();
                    _logger.LogError("Failed to load variants - Status: {StatusCode}", variantsResponse.StatusCode);
                }

                // Load Variant Attributes
                var attributesResponse = await httpClient.GetAsync("api/VariantAttribute/with-values");
                if (attributesResponse.IsSuccessStatusCode)
                {
                    var attributesJson = await attributesResponse.Content.ReadAsStringAsync();
                    VariantAttributes = JsonSerializer.Deserialize<List<VariantAttributeDto>>(attributesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<VariantAttributeDto>();
                    
                    int totalValues = VariantAttributes.Sum(a => a.Values?.Count ?? 0);
                    _logger.LogInformation("Loaded {AttributeCount} attributes with {ValueCount} total values", 
                        VariantAttributes.Count, totalValues);
                }
                else
                {
                    VariantAttributes = new List<VariantAttributeDto>();
                    _logger.LogError("Failed to load variant attributes - Status: {StatusCode}", attributesResponse.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during data loading for PhoneId: {PhoneId}", phoneId);
                
                // Initialize empty collections to prevent null reference exceptions
                Phone = new PhoneDto();
                Colors = new List<ColorDto>();
                PhoneVariants = new List<PhoneVariantDto>();
                VariantAttributes = new List<VariantAttributeDto>();
            }
        }

        private async Task GenerateVariantsFromCombinations()
        {
            _logger.LogInformation("Generating variants for {ColorCount} colors with {AttributeCount} attribute values", 
                SelectedColors.Count, SelectedAttributeValues.Count);
            
            var httpClient = _httpClientFactory.CreateClient("API");
            var token = GetTokenFromClaims();
            
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Authentication token missing");
                throw new UnauthorizedAccessException("Authentication token is missing. Please login again.");
            }

            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                // If no attribute values selected, just create variants for colors only
                if (!SelectedAttributeValues.Any())
                {
                    _logger.LogInformation("Creating {Count} color-only variants", SelectedColors.Count);
                    foreach (var colorId in SelectedColors)
                    {
                        await CreateVariantWithAttributes(httpClient, colorId, new List<int>());
                    }
                    return;
                }

                // Group attribute values by attribute ID
                var attributeGroups = new Dictionary<int, List<int>>();
                
                if (VariantAttributes == null || !VariantAttributes.Any())
                {
                    _logger.LogError("VariantAttributes is null or empty!");
                    throw new InvalidOperationException("VariantAttributes not loaded");
                }
                
                // 🔍 DEBUG: Log selected attribute values
                _logger.LogInformation("=== DEBUG: Selected Attribute Values ===");
                _logger.LogInformation("SelectedAttributeValues: [{ValueIds}]", string.Join(", ", SelectedAttributeValues));
                
                foreach (var valueId in SelectedAttributeValues)
                {
                    // 🔍 DEBUG: Log processing each value
                    _logger.LogInformation("Processing ValueId: {ValueId}", valueId);
                    
                    // Find which attribute this value belongs to
                    bool found = false;
                    foreach (var attr in VariantAttributes)
                    {
                        var value = attr.Values.FirstOrDefault(v => v.ValueId == valueId);
                        if (value != null)
                        {
                            // 🔍 DEBUG: Log when value is found
                            _logger.LogInformation("✅ ValueId {ValueId} ({ValueName}) belongs to Attribute {AttrId} ({AttrName})", 
                                valueId, value.Value, attr.VariantAttributeId, attr.Name);
                            
                            if (!attributeGroups.ContainsKey(attr.VariantAttributeId))
                                attributeGroups[attr.VariantAttributeId] = new List<int>();
                            
                            attributeGroups[attr.VariantAttributeId].Add(valueId);
                            found = true;
                            break;
                        }
                    }
                    
                    if (!found)
                    {
                        _logger.LogWarning("❌ Could not find attribute for value ID: {ValueId}", valueId);
                    }
                }

                _logger.LogInformation("Created {GroupCount} attribute groups", attributeGroups.Count);

                // Generate all combinations of attribute values
                var combinations = GenerateAttributeCombinations(attributeGroups);
                
                int totalVariants = SelectedColors.Count * Math.Max(1, combinations.Count);
                _logger.LogInformation("Will create {TotalVariants} variants ({ColorCount} colors × {CombinationCount} combinations)", 
                    totalVariants, SelectedColors.Count, Math.Max(1, combinations.Count));

                // Create variants for each color and combination
                int createdCount = 0;
                foreach (var colorId in SelectedColors)
                {
                    if (combinations.Any())
                    {
                        // Create one variant for each combination of this color
                        foreach (var combination in combinations)
                        {
                            await CreateVariantWithAttributes(httpClient, colorId, combination);
                            createdCount++;
                        }
                    }
                    else
                    {
                        // No attribute combinations, create one variant per color
                        await CreateVariantWithAttributes(httpClient, colorId, new List<int>());
                        createdCount++;
                    }
                }
                
                _logger.LogInformation("Successfully created {CreatedCount} variants", createdCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GenerateVariantsFromCombinations");
                throw;
            }
        }

        private async Task CreateVariantWithAttributes(HttpClient httpClient, int colorId, List<int> attributeValueIds)
        {
            // Generate unique SKU based on color and attributes
            var sku = GenerateVariantSku(colorId, attributeValueIds);
            
            // 🔍 DEBUG: Log variant creation attempt
            _logger.LogInformation("=== DEBUG: Creating Variant ===");
            _logger.LogInformation("ColorId: {ColorId}, AttributeValueIds: [{ValueIds}], SKU: {Sku}", 
                colorId, string.Join(", ", attributeValueIds), sku);
            
            // Create variant data for API call
            var variantData = new
            {
                PhoneId = PhoneId,
                ColorId = colorId,
                Price = BasePrice,
                Stock = BaseStock,
                Status = 1, // Active
                IsDefault = false,
                Sku = sku
            };

            var json = JsonSerializer.Serialize(variantData, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
            
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/PhoneVariant", content);
            
            // 🔍 DEBUG: Log API response
            _logger.LogInformation("Variant API Response - Status: {StatusCode}, SKU: {Sku}", response.StatusCode, sku);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("❌ FAILED to create variant - SKU: {Sku}, Status: {Status}, Error: {Error}", 
                    sku, response.StatusCode, errorContent);
                return;
            }

            _logger.LogInformation("✅ Created variant with SKU: {Sku}", sku);
            
            // Create VariantAttributeMappings if we have attribute values
            if (attributeValueIds.Any())
            {
                // 🔍 DEBUG: Log mapping creation attempt
                _logger.LogInformation("Creating mappings for variant with SKU: {Sku}, ValueIds: [{ValueIds}]", 
                    sku, string.Join(", ", attributeValueIds));
                await CreateVariantAttributeMappings(httpClient, response, attributeValueIds);
            }
        }

        private async Task CreateVariantAttributeMappings(HttpClient httpClient, HttpResponseMessage variantResponse, List<int> attributeValueIds)
        {
            try
            {
                // Parse the variant response to get VariantId
                var responseContent = await variantResponse.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    _logger.LogError("Empty variant response content");
                    return;
                }
                
                var createdVariant = JsonSerializer.Deserialize<PhoneVariantDto>(responseContent, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
                
                if (createdVariant == null || createdVariant.VariantId <= 0)
                {
                    _logger.LogError("Failed to parse variant response");
                    return;
                }

                _logger.LogInformation("Creating {Count} attribute mappings for VariantId: {VariantId}", 
                    attributeValueIds.Count, createdVariant.VariantId);
                
                int successCount = 0;
                
                foreach (var valueId in attributeValueIds)
                {
                    var mappingDto = new CreateVariantAttributeMappingDto
                    {
                        VariantId = createdVariant.VariantId,
                        ValueId = valueId
                    };

                    var json = JsonSerializer.Serialize(mappingDto, new JsonSerializerOptions 
                    { 
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                    });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    
                    var response = await httpClient.PostAsync("api/PhoneVariant/mapping", content);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        successCount++;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError("Failed to create mapping - VariantId: {VariantId}, ValueId: {ValueId}, Error: {Error}", 
                            createdVariant.VariantId, valueId, errorContent);
                    }
                }
                
                _logger.LogInformation("Created {Success}/{Total} attribute mappings", successCount, attributeValueIds.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while creating VariantAttributeMappings");
            }
        }

        private string GenerateVariantSku(int colorId, List<int> attributeValueIds)
        {
            // Get color name for SKU
            var colorName = Colors.FirstOrDefault(c => c.ColorId == colorId)?.ColorName ?? "UNK";
            var colorCode = colorName.Length >= 3 ? colorName[..3].ToUpper() : colorName.ToUpper();
            
            // Generate attribute code from valueIds
            var attributeCode = attributeValueIds.Any() 
                ? string.Join("", attributeValueIds.Select(id => id.ToString().PadLeft(2, '0')))
                : "00";
            
            // Add timestamp to ensure uniqueness
            var timestamp = DateTime.Now.ToString("HHmmss");
            
            // Format: PHONE{PhoneId}-{ColorCode}-{AttributeCode}-{Timestamp}
            return $"PHONE{PhoneId}-{colorCode}-{attributeCode}-{timestamp}";
        }

        private List<List<int>> GenerateAttributeCombinations(Dictionary<int, List<int>> attributeGroups)
        {
            var combinations = new List<List<int>>();
            
            if (!attributeGroups.Any())
            {
                return combinations;
            }

            var attributes = attributeGroups.Keys.ToList();
            
            // 🔍 DEBUG: Log attribute groups before generating combinations
            _logger.LogInformation("=== DEBUG: Attribute Groups ===");
            foreach (var group in attributeGroups)
            {
                _logger.LogInformation("Attribute {AttrId}: Values [{ValueIds}]", 
                    group.Key, string.Join(", ", group.Value));
            }
            
            // Generate Cartesian product of all attribute value combinations
            GenerateCombinationsRecursive(attributeGroups, attributes, 0, new List<int>(), combinations);
            
            // 🔍 DEBUG: Log all generated combinations
            _logger.LogInformation("=== DEBUG: Generated Combinations ===");
            for (int i = 0; i < combinations.Count; i++)
            {
                _logger.LogInformation("Combination {Index}: [{ValueIds}]", i + 1, string.Join(", ", combinations[i]));
            }
            
            _logger.LogInformation("Generated {CombinationCount} attribute combinations", combinations.Count);
            
            return combinations;
        }

        private void GenerateCombinationsRecursive(
            Dictionary<int, List<int>> attributeGroups,
            List<int> attributes,
            int currentIndex,
            List<int> currentCombination,
            List<List<int>> allCombinations)
        {
            if (currentIndex >= attributes.Count)
            {
                // We've built a complete combination, add it to the result
                allCombinations.Add(new List<int>(currentCombination));
                return;
            }

            var currentAttributeId = attributes[currentIndex];
            var values = attributeGroups[currentAttributeId];

            // Try each value for the current attribute
            foreach (var valueId in values)
            {
                currentCombination.Add(valueId);
                GenerateCombinationsRecursive(attributeGroups, attributes, currentIndex + 1, currentCombination, allCombinations);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        private string GetTokenFromClaims()
        {
            return User.FindFirst("Token")?.Value ?? string.Empty;
        }

        public async Task<IActionResult> OnGetTestApiAsync()
        {
            _logger.LogInformation("=== TESTING API CONNECTION ===");
            
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var token = GetTokenFromClaims();
                
                _logger.LogInformation("API Base URL: {BaseUrl}", httpClient.BaseAddress);
                _logger.LogInformation("Token Available: {TokenExists}", !string.IsNullOrEmpty(token));
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                // Test simple GET request first
                _logger.LogInformation("Testing GET request to api/Color/phone/{PhoneId}", PhoneId);
                var testResponse = await httpClient.GetAsync($"api/Color/phone/{PhoneId}");
                _logger.LogInformation("Test GET Response: {StatusCode}", testResponse.StatusCode);
                
                if (!testResponse.IsSuccessStatusCode)
                {
                    var errorContent = await testResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Test GET failed: {Error}", errorContent);
                }
                
                // Test POST with minimal data
                var testColorDto = new CreateColorDto
                {
                    PhoneId = PhoneId,
                    ColorName = "Test Color " + DateTime.Now.Ticks,
                    ImageUrl = "https://via.placeholder.com/300x200/FF0000/FFFFFF?text=Test"
                };
                
                var json = JsonSerializer.Serialize(testColorDto, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
                
                _logger.LogInformation("Test POST JSON: {Json}", json);
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                _logger.LogInformation("Testing POST request to api/Color");
                var postResponse = await httpClient.PostAsync("api/Color", content);
                _logger.LogInformation("Test POST Response: {StatusCode}", postResponse.StatusCode);
                
                var responseContent = await postResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("Test POST Response Content: {Content}", responseContent);
                
                return new JsonResult(new
                {
                    Success = postResponse.IsSuccessStatusCode,
                    StatusCode = postResponse.StatusCode.ToString(),
                    Response = responseContent,
                    BaseUrl = httpClient.BaseAddress?.ToString(),
                    TokenExists = !string.IsNullOrEmpty(token)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API Test failed with exception");
                return new JsonResult(new
                {
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        public IActionResult OnGetDebugAuth()
        {
            _logger.LogInformation("=== DEBUG AUTH INFO ===");
            _logger.LogInformation("User IsAuthenticated: {IsAuth}", User.Identity?.IsAuthenticated);
            _logger.LogInformation("User Identity Name: {Name}", User.Identity?.Name);
            _logger.LogInformation("User Claims Count: {ClaimsCount}", User.Claims.Count());
            
            foreach (var claim in User.Claims)
            {
                _logger.LogInformation("Claim - Type: {Type}, Value: {Value}", claim.Type, 
                    claim.Type == "Token" ? (claim.Value?.Length > 50 ? claim.Value[..50] + "..." : claim.Value) : claim.Value);
            }
            
            var token = GetTokenFromClaims();
            _logger.LogInformation("Token exists: {TokenExists}, Length: {TokenLength}", 
                !string.IsNullOrEmpty(token), token?.Length ?? 0);
                
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            _logger.LogInformation("User Role: {Role}", role);
            
            return new JsonResult(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated,
                Name = User.Identity?.Name,
                Role = role,
                TokenExists = !string.IsNullOrEmpty(token),
                TokenLength = token?.Length ?? 0,
                Claims = User.Claims.Select(c => new { c.Type, Value = c.Type == "Token" ? "***" : c.Value }).ToList()
            });
        }

        public string GetColorName(int? colorId)
        {
            if (!colorId.HasValue) return "Unknown";
            return Colors.FirstOrDefault(c => c.ColorId == colorId.Value)?.ColorName ?? "Unknown";
        }

        public async Task<IActionResult> OnGetCheckPhoneAsync(int phoneId)
        {
            _logger.LogInformation("=== CHECKING PHONE IN DATABASE ===");
            _logger.LogInformation("Checking PhoneID: {PhoneId}", phoneId);
            
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var token = GetTokenFromClaims();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                // Check if phone exists
                var phoneResponse = await httpClient.GetAsync($"api/Phone/{phoneId}");
                _logger.LogInformation("Phone check response: {StatusCode}", phoneResponse.StatusCode);
                
                if (phoneResponse.IsSuccessStatusCode)
                {
                    var phoneContent = await phoneResponse.Content.ReadAsStringAsync();
                    _logger.LogInformation("Phone exists: {PhoneData}", phoneContent);
                    
                    return new JsonResult(new
                    {
                        Exists = true,
                        StatusCode = phoneResponse.StatusCode.ToString(),
                        Phone = phoneContent
                    });
                }
                else
                {
                    var errorContent = await phoneResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Phone does not exist: {Error}", errorContent);
                    
                    return new JsonResult(new
                    {
                        Exists = false,
                        StatusCode = phoneResponse.StatusCode.ToString(),
                        Error = errorContent
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking phone existence");
                return new JsonResult(new
                {
                    Exists = false,
                    Error = ex.Message
                });
            }
        }

        public async Task<IActionResult> OnGetTestMappingAsync()
        {
            _logger.LogInformation("=== TESTING VARIANT ATTRIBUTE MAPPING API ===");
            
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var token = GetTokenFromClaims();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                // Test 1: Check available variants
                _logger.LogInformation("Step 1: Getting available variants");
                var variantsResponse = await httpClient.GetAsync("api/PhoneVariant/debug/variants");
                var variantsContent = await variantsResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("Variants response: {Status} - {Content}", variantsResponse.StatusCode, variantsContent);
                
                // Test 2: Check available variant attribute values
                _logger.LogInformation("Step 2: Getting available variant attribute values");
                var valuesResponse = await httpClient.GetAsync("api/PhoneVariant/debug/variant-attribute-values");
                var valuesContent = await valuesResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("Values response: {Status} - {Content}", valuesResponse.StatusCode, valuesContent);
                
                // Test 3: Try to create a test mapping
                _logger.LogInformation("Step 3: Creating test mapping");
                var testMapping = new CreateVariantAttributeMappingDto
                {
                    VariantId = 1, // Test with VariantId 1
                    ValueId = 1    // Test with ValueId 1
                };
                
                var json = JsonSerializer.Serialize(testMapping, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                _logger.LogInformation("Test mapping JSON: {Json}", json);
                
                var mappingResponse = await httpClient.PostAsync("api/PhoneVariant/mapping", content);
                var mappingContent = await mappingResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("Mapping response: {Status} - {Content}", mappingResponse.StatusCode, mappingContent);
                
                // Test 4: Get attributes for a variant
                _logger.LogInformation("Step 4: Getting attributes for VariantId 1");
                var attributesResponse = await httpClient.GetAsync("api/PhoneVariant/1/attributes");
                var attributesContent = await attributesResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("Attributes response: {Status} - {Content}", attributesResponse.StatusCode, attributesContent);
                
                return new JsonResult(new
                {
                    Success = true,
                    VariantsCheck = new { Status = variantsResponse.StatusCode.ToString(), Content = variantsContent },
                    ValuesCheck = new { Status = valuesResponse.StatusCode.ToString(), Content = valuesContent },
                    MappingTest = new { Status = mappingResponse.StatusCode.ToString(), Content = mappingContent },
                    AttributesCheck = new { Status = attributesResponse.StatusCode.ToString(), Content = attributesContent }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mapping test failed with exception");
                return new JsonResult(new
                {
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        public async Task<IActionResult> OnGetDebugVariantAttributesAsync()
        {
            _logger.LogInformation("=== COMPREHENSIVE VARIANT ATTRIBUTES DEBUG ===");
            
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var token = GetTokenFromClaims();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                var result = new
                {
                    Step1_BaseUrl = httpClient.BaseAddress?.ToString(),
                    Step2_TokenExists = !string.IsNullOrEmpty(token),
                    Step3_OldEndpoint = (object)null,
                    Step4_NewEndpoint = (object)null,
                    Step5_CurrentModel = (object)null,
                    Step6_SelectedValues = SelectedAttributeValues,
                    Step7_Summary = ""
                };
                
                // Step 3: Test old endpoint
                _logger.LogInformation("Step 3: Testing OLD endpoint /api/VariantAttribute");
                try
                {
                    var oldResponse = await httpClient.GetAsync("api/VariantAttribute");
                    var oldContent = await oldResponse.Content.ReadAsStringAsync();
                    _logger.LogInformation("Old endpoint - Status: {Status}, Content Length: {Length}", 
                        oldResponse.StatusCode, oldContent?.Length ?? 0);
                    
                    result = result with 
                    { 
                        Step3_OldEndpoint = new 
                        { 
                            Status = oldResponse.StatusCode.ToString(), 
                            ContentLength = oldContent?.Length ?? 0,
                            Content = oldContent?.Length > 200 ? oldContent[..200] + "..." : oldContent
                        } 
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Old endpoint failed");
                    result = result with { Step3_OldEndpoint = new { Error = ex.Message } };
                }
                
                // Step 4: Test new endpoint
                _logger.LogInformation("Step 4: Testing NEW endpoint /api/VariantAttribute/with-values");
                try
                {
                    var newResponse = await httpClient.GetAsync("api/VariantAttribute/with-values");
                    var newContent = await newResponse.Content.ReadAsStringAsync();
                    _logger.LogInformation("New endpoint - Status: {Status}, Content Length: {Length}", 
                        newResponse.StatusCode, newContent?.Length ?? 0);
                    
                    result = result with 
                    { 
                        Step4_NewEndpoint = new 
                        { 
                            Status = newResponse.StatusCode.ToString(), 
                            ContentLength = newContent?.Length ?? 0,
                            Content = newContent?.Length > 200 ? newContent[..200] + "..." : newContent,
                            FullContent = newContent
                        } 
                    };
                    
                    // Try to parse the response
                    if (newResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(newContent))
                    {
                        try
                        {
                            var parsed = JsonSerializer.Deserialize<List<VariantAttributeDto>>(newContent, 
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            _logger.LogInformation("Successfully parsed {Count} attributes from new endpoint", parsed?.Count ?? 0);
                        }
                        catch (Exception parseEx)
                        {
                            _logger.LogError(parseEx, "Failed to parse new endpoint response");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "New endpoint failed");
                    result = result with { Step4_NewEndpoint = new { Error = ex.Message } };
                }
                
                // Step 5: Current model data
                _logger.LogInformation("Step 5: Current VariantAttributes in model");
                var modelAttributes = VariantAttributes?.Select(a => new 
                {
                    a.VariantAttributeId,
                    a.Name,
                    ValuesCount = a.Values?.Count ?? 0,
                    Values = a.Values?.Select(v => new { v.ValueId, v.Value }).ToList()
                }).ToList();
                
                result = result with 
                { 
                    Step5_CurrentModel = new
                    {
                        Count = VariantAttributes?.Count ?? 0,
                        Attributes = modelAttributes
                    }
                };
                
                // Step 7: Summary
                var summary = "";
                if (VariantAttributes == null || !VariantAttributes.Any())
                {
                    summary = "❌ NO VARIANT ATTRIBUTES LOADED IN MODEL";
                }
                else if (!VariantAttributes.Any(a => a.Values?.Any() == true))
                {
                    summary = "❌ VARIANT ATTRIBUTES LOADED BUT NO VALUES";
                }
                else
                {
                    var totalValues = VariantAttributes.Sum(a => a.Values?.Count ?? 0);
                    summary = $"✅ {VariantAttributes.Count} attributes with {totalValues} total values loaded";
                }
                
                result = result with { Step7_Summary = summary };
                
                _logger.LogInformation("Debug summary: {Summary}", summary);
                
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Comprehensive debug failed");
                return new JsonResult(new
                {
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        public async Task<IActionResult> OnGetSimpleCheckAsync()
        {
            _logger.LogInformation("=== SIMPLE DATABASE CHECK ===");
            
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var token = GetTokenFromClaims();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                // Check VariantAttribute count
                var response = await httpClient.GetAsync("api/VariantAttribute");
                var content = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation("VariantAttribute API Response: {Status}", response.StatusCode);
                _logger.LogInformation("Content: {Content}", content);
                
                var count = 0;
                var hasValues = false;
                
                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(content))
                {
                    try
                    {
                        var attributes = JsonSerializer.Deserialize<JsonElement[]>(content);
                        count = attributes?.Length ?? 0;
                        _logger.LogInformation("Found {Count} VariantAttributes in database", count);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to parse response");
                    }
                }
                
                // Check with-values endpoint
                var valuesResponse = await httpClient.GetAsync("api/VariantAttribute/with-values");
                var valuesContent = await valuesResponse.Content.ReadAsStringAsync();
                
                _logger.LogInformation("With-values API Response: {Status}", valuesResponse.StatusCode);
                
                if (valuesResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(valuesContent))
                {
                    hasValues = valuesContent.Contains("values") && valuesContent.Length > 10;
                }
                
                var diagnosis = "";
                if (count == 0)
                {
                    diagnosis = "❌ NO VARIANT ATTRIBUTES IN DATABASE - Need to create VariantAttributes first!";
                }
                else if (!hasValues)
                {
                    diagnosis = $"⚠️ Found {count} VariantAttributes but NO VALUES - Need to create VariantAttributeValues!";
                }
                else
                {
                    diagnosis = $"✅ Found {count} VariantAttributes with values - Should work!";
                }
                
                return new JsonResult(new
                {
                    AttributeCount = count,
                    HasValues = hasValues,
                    Diagnosis = diagnosis,
                    RawResponse = content,
                    ValuesResponse = valuesContent
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Simple check failed");
                return new JsonResult(new
                {
                    Error = ex.Message,
                    Diagnosis = "❌ API CONNECTION FAILED"
                });
            }
        }

        public async Task<IActionResult> OnGetCheckValuesAsync()
        {
            _logger.LogInformation("=== CHECKING VARIANT ATTRIBUTE VALUES ===");
            
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var token = GetTokenFromClaims();
                
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                // Get all variant attributes with values
                var response = await httpClient.GetAsync("api/VariantAttribute/with-values");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var attributes = JsonSerializer.Deserialize<List<VariantAttributeDto>>(content, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    var result = new
                    {
                        Success = true,
                        AttributeCount = attributes?.Count ?? 0,
                        Attributes = attributes?.Select(a => new 
                        {
                            a.VariantAttributeId,
                            a.Name,
                            Values = a.Values?.Select(v => new 
                            {
                                v.ValueId,
                                v.Value,
                                Contains256 = v.Value.Contains("256", StringComparison.OrdinalIgnoreCase)
                            }).ToList()
                        }).ToList(),
                        StorageValues = attributes
                            ?.FirstOrDefault(a => a.Name.Contains("Storage", StringComparison.OrdinalIgnoreCase))
                            ?.Values?.Select(v => $"ValueId: {v.ValueId}, Value: '{v.Value}'").ToList(),
                        AllValues = attributes?.SelectMany(a => a.Values ?? new List<VariantAttributeValueDto>())
                            .Select(v => $"ValueId: {v.ValueId}, Value: '{v.Value}'").ToList()
                    };
                    
                    _logger.LogInformation("Found {Count} attributes with values", attributes?.Count ?? 0);
                    _logger.LogInformation("Storage values: {StorageValues}", string.Join(", ", result.StorageValues ?? new List<string>()));
                    
                    return new JsonResult(result);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new JsonResult(new
                    {
                        Success = false,
                        Error = errorContent,
                        Status = response.StatusCode.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking values");
                return new JsonResult(new
                {
                    Success = false,
                    Error = ex.Message
                });
            }
        }

        public async Task<IActionResult> OnPostEditVariantAsync()
        {
            _logger.LogInformation("=== Starting Edit Variant ===");
            _logger.LogInformation("EditVariantId: {VariantId}, EditPrice: {Price}, EditStock: {Stock}", 
                EditVariantId, EditPrice, EditStock);
            
            // Clear validation errors for fields not used in EditVariant
            ModelState.Remove(nameof(ColorName));
            ModelState.Remove(nameof(ImageUrl));
            ModelState.Remove(nameof(BasePrice));
            ModelState.Remove(nameof(BaseStock));
            ModelState.Remove(nameof(SelectedColors));
            ModelState.Remove(nameof(SelectedAttributeValues));
            
            // Manual validation for EditVariant specific fields
            if (EditVariantId <= 0)
            {
                ModelState.AddModelError(nameof(EditVariantId), "Valid Variant ID is required");
            }
            
            if (EditPrice <= 0)
            {
                ModelState.AddModelError(nameof(EditPrice), "Price must be greater than 0");
            }
            
            if (EditStock < 0)
            {
                ModelState.AddModelError(nameof(EditStock), "Stock cannot be negative");
            }
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation failed for EditVariant");
                await LoadDataAsync(PhoneId);
                return Page();
            }

            // Get PhoneId from route parameter
            var routePhoneId = RouteData.Values["id"]?.ToString();
            if (int.TryParse(routePhoneId, out int parsedPhoneId))
            {
                PhoneId = parsedPhoneId;
            }
            else
            {
                _logger.LogError("Invalid PhoneId in route: {RouteId}", routePhoneId);
                ErrorMessage = "Invalid Phone ID in URL";
                return RedirectToPage("/Admin/Phone/Index");
            }

            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var token = GetTokenFromClaims();
                
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Authentication token missing");
                    ErrorMessage = "Authentication token is missing. Please login again.";
                    await LoadDataAsync(PhoneId);
                    return Page();
                }

                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Get current variant data first to preserve other fields
                var getResponse = await httpClient.GetAsync($"api/PhoneVariant/{EditVariantId}");
                if (!getResponse.IsSuccessStatusCode)
                {
                    var getError = await getResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to get current variant data - Status: {Status}, Error: {Error}", 
                        getResponse.StatusCode, getError);
                    ErrorMessage = "Failed to get current variant data";
                    await LoadDataAsync(PhoneId);
                    return Page();
                }

                var currentVariantJson = await getResponse.Content.ReadAsStringAsync();
                var currentVariant = JsonSerializer.Deserialize<PhoneVariantDto>(currentVariantJson, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (currentVariant == null)
                {
                    _logger.LogError("Failed to parse current variant data");
                    ErrorMessage = "Failed to parse current variant data";
                    await LoadDataAsync(PhoneId);
                    return Page();
                }

                // Create complete update data with only modified fields
                var updateData = new
                {
                    Price = EditPrice,
                    Stock = EditStock,
                    Sku = currentVariant.Sku, // Preserve existing SKU
                    Status = currentVariant.Status, // Preserve existing status
                    IsDefault = currentVariant.IsDefault // Preserve existing IsDefault
                };

                var json = JsonSerializer.Serialize(updateData, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                _logger.LogInformation("Updating variant {VariantId} with data: {Data}", EditVariantId, json);
                
                var response = await httpClient.PutAsync($"api/PhoneVariant/{EditVariantId}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Variant updated successfully");
                    SuccessMessage = "Variant updated successfully!";
                    return RedirectToPage(new { id = PhoneId });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to update variant - Status: {Status}, Error: {Error}", 
                        response.StatusCode, errorContent);
                    ErrorMessage = $"Failed to update variant: {errorContent}";
                    await LoadDataAsync(PhoneId);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating variant: {Message}", ex.Message);
                ErrorMessage = $"Error updating variant: {ex.Message}";
                await LoadDataAsync(PhoneId);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteVariantAsync(int variantId)
        {
            _logger.LogInformation("=== Starting Delete Variant ===");
            _logger.LogInformation("VariantId: {VariantId}", variantId);
            
            // Get PhoneId from route parameter
            var routePhoneId = RouteData.Values["id"]?.ToString();
            if (int.TryParse(routePhoneId, out int parsedPhoneId))
            {
                PhoneId = parsedPhoneId;
            }
            else
            {
                _logger.LogError("Invalid PhoneId in route: {RouteId}", routePhoneId);
                ErrorMessage = "Invalid Phone ID in URL";
                return RedirectToPage("/Admin/Phone/Index");
            }

            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var token = GetTokenFromClaims();
                
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Authentication token missing");
                    ErrorMessage = "Authentication token is missing. Please login again.";
                    return RedirectToPage(new { id = PhoneId });
                }

                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                _logger.LogInformation("Sending DELETE request to api/PhoneVariant/{VariantId}", variantId);
                var response = await httpClient.DeleteAsync($"api/PhoneVariant/{variantId}");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Variant deleted successfully");
                    SuccessMessage = "Variant deleted successfully!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to delete variant - Status: {Status}, Error: {Error}", 
                        response.StatusCode, errorContent);
                    ErrorMessage = "Failed to delete variant. Please try again.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting variant: {Message}", ex.Message);
                ErrorMessage = $"Error deleting variant: {ex.Message}";
            }

            return RedirectToPage(new { id = PhoneId });
        }
    }
} 