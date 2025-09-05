using CellPhoneShop.Web.DTOs.Phone;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace CellPhoneShop.Web.Pages.Admin.Phone
{
    [Authorize(Roles = "Admin,Staff")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger;

        public List<PhoneDto> Phones { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public PhoneSearchResultDto PhonesResult { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems => PhonesResult?.TotalCount ?? 0;
        public int TotalPages => PhonesResult?.TotalPages ?? 1;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        [BindProperty(SupportsGet = true)]
        public int? BrandId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortDirection { get; set; } = "desc";

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

                _logger.LogInformation("Token: {Token}", token);
                foreach (var header in client.DefaultRequestHeaders)
                {
                    _logger.LogInformation("Header: {Key} = {Value}", header.Key, string.Join(",", header.Value));
                }
                var queryParams = new List<string>();
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                    queryParams.Add($"searchTerm={Uri.EscapeDataString(SearchTerm)}");
                if (BrandId.HasValue)
                    queryParams.Add($"brandId={BrandId}");
                if (!string.IsNullOrWhiteSpace(SortBy))
                    queryParams.Add($"sortBy={SortBy}");
                if (!string.IsNullOrWhiteSpace(SortDirection))
                    queryParams.Add($"sortDirection={SortDirection}");
                queryParams.Add($"page={CurrentPage}");
                queryParams.Add($"pageSize={PageSize}");

                var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                var response = await client.GetAsync($"/api/Phone/search{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    PhonesResult = JsonSerializer.Deserialize<PhoneSearchResultDto>(content, options);
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
                var client = _httpClientFactory.CreateClient("API");
                var token = User.FindFirst("Token")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

            return RedirectToPage(new { SearchTerm, BrandId, SortBy, SortDirection, CurrentPage });
        }

        public string GetPageUrl(int pageNumber)
        {
            var queryParams = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(SearchTerm))
                queryParams["SearchTerm"] = SearchTerm;
            if (BrandId.HasValue)
                queryParams["BrandId"] = BrandId.ToString();
            if (!string.IsNullOrWhiteSpace(SortBy))
                queryParams["SortBy"] = SortBy;
            if (!string.IsNullOrWhiteSpace(SortDirection))
                queryParams["SortDirection"] = SortDirection;
            queryParams["CurrentPage"] = pageNumber.ToString();
            return Url.Page("./Index", queryParams);
        }
    }
} 