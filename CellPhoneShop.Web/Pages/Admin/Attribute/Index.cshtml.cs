using CellPhoneShop.Business.DTOs.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace CellPhoneShop.Web.Pages.Admin.Attribute
{
    [Authorize(Roles = "Admin,Staff")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger;

        public List<PhoneAttributeDto> Attributes { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortDirection { get; set; } = "asc";

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var queryParams = new List<string>();
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                    queryParams.Add($"searchTerm={Uri.EscapeDataString(SearchTerm)}");
                if (!string.IsNullOrWhiteSpace(SortBy))
                    queryParams.Add($"sortBy={SortBy}");
                if (!string.IsNullOrWhiteSpace(SortDirection))
                    queryParams.Add($"sortDirection={SortDirection}");
                queryParams.Add($"page={CurrentPage}");
                queryParams.Add($"pageSize={PageSize}");

                var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                var response = await client.GetAsync($"/api/PhoneAttribute{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Attributes = JsonSerializer.Deserialize<List<PhoneAttributeDto>>(content, options) ?? new();
                }
                else
                {
                    _logger.LogError("API returned status code: {StatusCode}", response.StatusCode);
                    ErrorMessage = $"Error loading attributes. Status code: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching attributes");
                ErrorMessage = "An error occurred while loading the attributes. Please try again later.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.DeleteAsync($"/api/PhoneAttribute/{id}");
                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Attribute deleted successfully.";
                }
                else
                {
                    ErrorMessage = "Failed to delete the attribute.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting attribute");
                ErrorMessage = "An error occurred while deleting the attribute.";
            }

            return RedirectToPage(new { SearchTerm, SortBy, SortDirection, CurrentPage });
        }
    }
} 