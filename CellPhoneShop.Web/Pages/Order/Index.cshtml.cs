    using CellPhoneShop.Web.DTOs.Order.OrderDtos;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json;
    using System.Text;
    using Microsoft.AspNetCore.Authorization;

    namespace CellPhoneShop.Web.Pages.Order
    {
        [Authorize(Roles = "Staff,Admin")]
        public class IndexModel : PageModel
        {
            private readonly IHttpClientFactory _clientFactory;
            private readonly ILogger<IndexModel> _logger;

            public List<OrderDto> Orders { get; set; } = new();
            public string ErrorMessage { get; set; }
            public string SuccessMessage { get; set; }

            [BindProperty(SupportsGet = true)]
            public string? SearchTerm { get; set; }

            public OrderSearchResultDto OrdersResult { get; set; } = new();
            [BindProperty(SupportsGet = true)]
            public int CurrentPage { get; set; } = 1;
            public int PageSize { get; set; } = 10;
            public int TotalItems => OrdersResult?.TotalCount ?? 0;
            public int TotalPages => OrdersResult?.TotalPages ?? 1;
            public bool HasPreviousPage => CurrentPage > 1;
            public bool HasNextPage => CurrentPage < TotalPages;
            [BindProperty(SupportsGet = true)]
            public int? Status { get; set; }
            [BindProperty(SupportsGet = true)]
            public string? SortBy { get; set; }
            [BindProperty(SupportsGet = true)]
            public string? SortDirection { get; set; } = "desc";

            // Order status options
            public Dictionary<int, string> OrderStatusOptions { get; set; } = new()
            {
                { 0, "Pending" },
                { 1, "Processing" },
                { 2, "Shipped" },
                { 3, "Delivered" },
                { 4, "Cancelled" }
            };

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
                    var token = User.FindFirst("Token")?.Value;
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }
                    var queryParams = new List<string>();
                    if (!string.IsNullOrWhiteSpace(SearchTerm))
                        queryParams.Add($"searchTerm={Uri.EscapeDataString(SearchTerm)}");
                    if (Status.HasValue)
                        queryParams.Add($"status={Status}");
                    if (!string.IsNullOrWhiteSpace(SortBy))
                        queryParams.Add($"sortBy={SortBy}");
                    if (!string.IsNullOrWhiteSpace(SortDirection))
                        queryParams.Add($"sortDirection={SortDirection}");
                    queryParams.Add($"page={CurrentPage}");
                    queryParams.Add($"pageSize={PageSize}");
                    var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                    var response = await client.GetAsync($"/api/Orders/search{queryString}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        OrdersResult = System.Text.Json.JsonSerializer.Deserialize<OrderSearchResultDto>(content, options);
                    }
                    else
                    {
                        _logger.LogError("API returned status code: {StatusCode}", response.StatusCode);
                        ErrorMessage = $"Error loading orders. Status code: {response.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while fetching orders");
                    ErrorMessage = "An error occurred while loading the orders. Please try again later.";
                }
                return Page();
            }

            public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, int newStatus)
            {
                try
                {
                    var client = _clientFactory.CreateClient("API");
                    var token = User.FindFirst("Token")?.Value;
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }

                    // Create the request body for status update
                    var updateRequest = new { Status = newStatus };
                    var json = JsonSerializer.Serialize(updateRequest);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync($"/api/Orders/{orderId}/status", content);
                
                    if (response.IsSuccessStatusCode)
                    {
                        SuccessMessage = "Order status updated successfully.";
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError("Failed to update order status. Status: {StatusCode}, Content: {Content}", 
                            response.StatusCode, errorContent);
                        ErrorMessage = "Failed to update the order status.";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating order status");
                    ErrorMessage = "An error occurred while updating the order status.";
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

                    var response = await client.DeleteAsync($"/api/Orders/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        SuccessMessage = "Order deleted successfully.";
                    }
                    else
                    {
                        ErrorMessage = "Failed to delete the order.";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting order");
                    ErrorMessage = "An error occurred while deleting the order.";
                }

                return RedirectToPage(new { SearchTerm });
            }

            public string GetStatusDisplayName(int status)
            {
                return OrderStatusOptions.TryGetValue(status, out var displayName) ? displayName : "Unknown";
            }

            public string GetPageUrl(int pageNumber)
            {
                var queryParams = new Dictionary<string, string>();
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                    queryParams["SearchTerm"] = SearchTerm;
                if (Status.HasValue)
                    queryParams["Status"] = Status.ToString();
                if (!string.IsNullOrWhiteSpace(SortBy))
                    queryParams["SortBy"] = SortBy;
                if (!string.IsNullOrWhiteSpace(SortDirection))
                    queryParams["SortDirection"] = SortDirection;
                queryParams["CurrentPage"] = pageNumber.ToString();
                return Url.Page("./Index", queryParams);
            }
        }
    }