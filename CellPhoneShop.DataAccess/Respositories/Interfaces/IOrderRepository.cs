using CellPhoneShop.Domain.Models;
using CellPhoneShop.DataAccess.Respositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Respositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task UpdateStatusAsync(int orderId, int status);
        // Add search and pagination methods
        Task<(IEnumerable<Order> Items, int TotalCount)> SearchOrdersAsync(
            string? searchTerm = null,
            int? userId = null,
            int? status = null,
            decimal? minAmount = null,
            decimal? maxAmount = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? paymentMethod = null,
            int page = 1,
            int pageSize = 10,
            string? sortBy = null,
            string? sortDirection = null);
    }
}
