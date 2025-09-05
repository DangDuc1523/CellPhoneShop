using CellPhoneShop.Web.DTOs.Order.OrderDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CellPhoneShop.Web.Pages.Admin.Order;

[Authorize(Roles = "Staff,Admin")]
public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

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

    public Dictionary<int, string> OrderStatusOptions { get; set; } = new()
    {
        { 0, "Pending" },
        { 1, "Processing" },
        { 2, "Shipped" },
        { 3, "Delivered" },
        { 4, "Cancelled" }
    };

    public IndexModel(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var token = User.FindFirst("Token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Orders/search{queryString}";
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                OrdersResult = JsonSerializer.Deserialize<OrderSearchResultDto>(content, options);
            }
            else
            {
                ErrorMessage = $"Error loading orders. Status code: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "An error occurred while loading the orders. Please try again later.";
        }
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, int newStatus)
    {
        try
        {
            var token = User.FindFirst("Token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var updateRequest = new { Status = newStatus };
            var json = JsonSerializer.Serialize(updateRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Orders/{orderId}/status";
            var response = await _httpClient.PutAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Order status updated successfully.";
            }
            else
            {
                ErrorMessage = "Failed to update the order status.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "An error occurred while updating the order status.";
        }
        return RedirectToPage(new { SearchTerm, Status, SortBy, SortDirection, CurrentPage });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            var token = User.FindFirst("Token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var apiUrl = $"{_configuration["ApiBaseUrl"]}/api/Orders/{id}";
            var response = await _httpClient.DeleteAsync(apiUrl);
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