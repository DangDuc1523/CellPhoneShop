using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using CellPhoneShop.Web.DTOs.Phone;
using CellPhoneShop.Web.DTOs.Brand;

namespace CellPhoneShop.Web.Pages.Phone
{
    [Authorize(Roles = "Staff,Admin")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<IndexModel> _logger;

        public List<PhoneDto> Phones { get; set; } = new();
        public List<BrandDto> Brands { get; set; } = new();
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? BrandId { get; set; }

        [BindProperty(SupportsGet = true)]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal? MinPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal? MaxPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Ram { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public IndexModel(IHttpClientFactory clientFactory, ILogger<IndexModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var client = _clientFactory.CreateClient("API");
                
                // Add JWT token to request header
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                // Get brands for dropdown
                var brandsResponse = await client.GetAsync("/api/Brand");
                if (brandsResponse.IsSuccessStatusCode)
                {
                    var content = await brandsResponse.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Brands = JsonSerializer.Deserialize<List<BrandDto>>(content, options);
                }

                // Get phones with search and pagination
                var searchDto = new PhoneSearchDto
                {
                    SearchTerm = SearchTerm,
                    BrandId = BrandId,
                    MinPrice = MinPrice,
                    MaxPrice = MaxPrice,
                    Ram = Ram,
                    Page = CurrentPage,
                    PageSize = PageSize
                };

                var queryParams = new List<string>();
                if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
                    queryParams.Add($"searchTerm={Uri.EscapeDataString(searchDto.SearchTerm)}");
                if (searchDto.BrandId.HasValue)
                    queryParams.Add($"brandId={searchDto.BrandId}");
                if (searchDto.MinPrice.HasValue)
                    queryParams.Add($"minPrice={searchDto.MinPrice}");
                if (searchDto.MaxPrice.HasValue)
                    queryParams.Add($"maxPrice={searchDto.MaxPrice}");
                if (!string.IsNullOrWhiteSpace(searchDto.Ram))
                    queryParams.Add($"ram={Uri.EscapeDataString(searchDto.Ram)}");
                queryParams.Add($"page={searchDto.Page}");
                queryParams.Add($"pageSize={searchDto.PageSize}");

                var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                var response = await client.GetAsync($"/api/Phone/search{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("API Response: {Content}", content);

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<PhoneSearchResultDto>(content, options);
                    
                    Phones = result.Items.ToList();
                    TotalItems = result.TotalCount;
                    CurrentPage = result.CurrentPage;
                    PageSize = result.PageSize;
                    TotalPages = result.TotalPages;
                }
                else
                {
                    _logger.LogError("API returned status code: {StatusCode}", response.StatusCode);
                    ErrorMessage = $"Error loading phones. Status code: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching phones");
                ErrorMessage = "An error occurred while loading the phones. Please try again later.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var client = _clientFactory.CreateClient("API");
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.DeleteAsync($"/api/Phone/{id}");
                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Phone deleted successfully.";
                }
                else
                {
                    ErrorMessage = "Failed to delete the phone.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting phone");
                ErrorMessage = "An error occurred while deleting the phone.";
            }

            return RedirectToPage();
        }

        public string GetPageUrl(int pageNumber)
        {
            var queryParams = new Dictionary<string, string>();
            
            if (!string.IsNullOrWhiteSpace(SearchTerm))
                queryParams["SearchTerm"] = SearchTerm;
            if (BrandId.HasValue)
                queryParams["BrandId"] = BrandId.ToString();
            if (MinPrice.HasValue)
                queryParams["MinPrice"] = MinPrice.ToString();
            if (MaxPrice.HasValue)
                queryParams["MaxPrice"] = MaxPrice.ToString();
            if (!string.IsNullOrWhiteSpace(Ram))
                queryParams["Ram"] = Ram;
            
            queryParams["CurrentPage"] = pageNumber.ToString();

            return Url.Page("./Index", queryParams);
        }
    }
} 