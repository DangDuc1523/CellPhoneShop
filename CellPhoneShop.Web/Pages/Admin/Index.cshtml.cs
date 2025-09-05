using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace CellPhoneShop.Web.Pages.Admin;

[Authorize(Roles = "Admin,Staff")]
public class IndexModel : PageModel
{
    public int TotalOrders { get; set; } = 0;
    public decimal TotalRevenue { get; set; } = 0;
    public int TotalProducts { get; set; } = 0;
    public int TotalCustomers { get; set; } = 0;
    public List<RecentOrder> RecentOrders { get; set; } = new();

    public void OnGet()
    {
        // Placeholder data - will be replaced with API calls
        TotalOrders = 156;
        TotalRevenue = 125000.50m;
        TotalProducts = 45;
        TotalCustomers = 89;

        RecentOrders = new List<RecentOrder>
        {
            new() { OrderId = 1001, CustomerName = "John Doe", TotalAmount = 999.99m, Status = "Completed", OrderDate = DateTime.Now.AddDays(-1) },
            new() { OrderId = 1002, CustomerName = "Jane Smith", TotalAmount = 1299.99m, Status = "Pending", OrderDate = DateTime.Now.AddDays(-2) },
            new() { OrderId = 1003, CustomerName = "Bob Johnson", TotalAmount = 799.99m, Status = "Completed", OrderDate = DateTime.Now.AddDays(-3) },
            new() { OrderId = 1004, CustomerName = "Alice Brown", TotalAmount = 1499.99m, Status = "Processing", OrderDate = DateTime.Now.AddDays(-4) },
            new() { OrderId = 1005, CustomerName = "Charlie Wilson", TotalAmount = 899.99m, Status = "Completed", OrderDate = DateTime.Now.AddDays(-5) }
        };
    }

    public class RecentOrder
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }
} 