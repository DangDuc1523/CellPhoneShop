using System.Text.Json;

namespace CellPhoneShop.Web.Services
{
    public interface IODataCustomerService
    {
        Task<List<PhoneVariantODataDto>> GetPhoneVariantsAsync(string? filter = null, string? orderBy = null, int? top = null, int? skip = null);
        Task<List<PhoneVariantODataDto>> GetPhoneVariantsByBrandAsync(int brandId, int? top = null);
        Task<List<PhoneVariantODataDto>> GetAvailablePhoneVariantsAsync(int? top = null);
        Task<List<PhoneVariantODataDto>> GetDiscountedPhoneVariantsAsync(int? top = null);
        Task<PhoneVariantODataDto?> GetPhoneVariantByIdAsync(int variantId);
        Task<List<PhoneVariantODataDto>> SearchPhoneVariantsAsync(string searchTerm, decimal? minPrice = null, decimal? maxPrice = null);
    }

    public class ODataCustomerService : IODataCustomerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ODataCustomerService> _logger;

        public ODataCustomerService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<ODataCustomerService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<PhoneVariantODataDto>> GetPhoneVariantsAsync(string? filter = null, string? orderBy = null, int? top = null, int? skip = null)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(filter)) queryParams.Add($"$filter={Uri.EscapeDataString(filter)}");
                if (!string.IsNullOrEmpty(orderBy)) queryParams.Add($"$orderby={Uri.EscapeDataString(orderBy)}");
                if (top.HasValue) queryParams.Add($"$top={top}");
                if (skip.HasValue) queryParams.Add($"$skip={skip}");
                
                var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
                var url = $"odata/customer/phonevariants{queryString}";
                
                _logger.LogInformation("Calling OData API: {Url}", url);
                
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ODataResponse<PhoneVariantODataDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                return result?.Value ?? new List<PhoneVariantODataDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OData API");
                return new List<PhoneVariantODataDto>();
            }
        }

        public async Task<List<PhoneVariantODataDto>> GetPhoneVariantsByBrandAsync(int brandId, int? top = null)
        {
            var filter = $"BrandId eq {brandId}";
            return await GetPhoneVariantsAsync(filter: filter, top: top);
        }

        public async Task<List<PhoneVariantODataDto>> GetAvailablePhoneVariantsAsync(int? top = null)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var queryString = top.HasValue ? $"?$top={top}" : "";
                var url = $"odata/customer/phonevariants/available{queryString}";
                
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ODataResponse<PhoneVariantODataDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                return result?.Value ?? new List<PhoneVariantODataDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OData API for available variants");
                return new List<PhoneVariantODataDto>();
            }
        }

        public async Task<List<PhoneVariantODataDto>> GetDiscountedPhoneVariantsAsync(int? top = null)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var queryString = top.HasValue ? $"?$top={top}" : "";
                var url = $"odata/customer/phonevariants/discounted{queryString}";
                
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ODataResponse<PhoneVariantODataDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                return result?.Value ?? new List<PhoneVariantODataDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OData API for discounted variants");
                return new List<PhoneVariantODataDto>();
            }
        }

        public async Task<PhoneVariantODataDto?> GetPhoneVariantByIdAsync(int variantId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("API");
                var url = $"odata/customer/phonevariants({variantId})";
                
                var response = await httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;
                    
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PhoneVariantODataDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OData API for variant {VariantId}", variantId);
                return null;
            }
        }

        public async Task<List<PhoneVariantODataDto>> SearchPhoneVariantsAsync(string searchTerm, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var filters = new List<string>();
            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filters.Add($"(contains(tolower(PhoneName), '{searchTerm.ToLower()}') or contains(tolower(BrandName), '{searchTerm.ToLower()}') or contains(tolower(ColorName), '{searchTerm.ToLower()}'))");
            }
            
            if (minPrice.HasValue)
            {
                filters.Add($"Price ge {minPrice}");
            }
            
            if (maxPrice.HasValue)
            {
                filters.Add($"Price le {maxPrice}");
            }
            
            var filter = filters.Any() ? string.Join(" and ", filters) : null;
            return await GetPhoneVariantsAsync(filter: filter, orderBy: "Price asc");
        }
    }

    // Helper classes for OData response
    public class ODataResponse<T>
    {
        public List<T> Value { get; set; } = new();
        public int? Count { get; set; }
        public string? NextLink { get; set; }
    }

    // DTO classes for Web layer
    public class PhoneVariantODataDto
    {
        public int VariantId { get; set; }
        public int PhoneId { get; set; }
        public int? ColorId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Sku { get; set; }
        public int Status { get; set; }
        public bool IsDefault { get; set; }
        public DateTime? CreatedAt { get; set; }
        
        public string PhoneName { get; set; } = string.Empty;
        public string? PhoneDescription { get; set; }
        public decimal PhoneBasePrice { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public int BrandId { get; set; }
        
        public string? ColorName { get; set; }
        public string? ColorImageUrl { get; set; }
        
        public decimal DiscountPercentage { get; set; }
        public bool IsInStock { get; set; }
        public bool IsActive { get; set; }
    }
} 