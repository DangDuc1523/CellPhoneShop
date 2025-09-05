using CellPhoneShop.DataAccess.Respositories.Interfaces;
using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CellPhoneShopContext _context;

        public OrderRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            // Remove order
            _context.Orders.Remove(order!);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                        .Include(o => o.OrderDetails)
                         
                        .FirstOrDefaultAsync(o => o.OrderId == id);

            return order!;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o =>o.UserId == userId)
                .ToListAsync();

                return orders;
         }

        public async Task UpdateStatusAsync(int orderId,int status)
        {
            var existingOrder = await _context.Orders.FindAsync(orderId);
            if (existingOrder != null)
            {
                // Update specific properties
                existingOrder.Status = status;
               
                // Add other properties you want to update

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Order with ID {orderId} not found.");
            }
        }
        public async Task<(IEnumerable<Order> Items, int TotalCount)> SearchOrdersAsync(
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
          string? sortDirection = null)
        {
            var query = _context.Orders
                .Include(o => o.OrderDetails)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(o =>
                    o.AddressDetail!.Contains(searchTerm) ||
                    o.Note!.Contains(searchTerm) ||
                    o.OrderId.ToString().Contains(searchTerm));
            }

            if (userId.HasValue)
            {
                query = query.Where(o => o.UserId == userId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            if (minAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount <= maxAmount.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                var endDate = toDate.Value.Date.AddDays(1); // Include the entire day
                query = query.Where(o => o.OrderDate < endDate);
            }

            if (paymentMethod.HasValue)
            {
                query = query.Where(o => o.PaymentMethod == paymentMethod.Value);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply dynamic sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                bool descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (sortBy.ToLower())
                {
                    case "orderdate":
                        query = descending ? query.OrderByDescending(o => o.OrderDate) : query.OrderBy(o => o.OrderDate);
                        break;
                    case "totalamount":
                        query = descending ? query.OrderByDescending(o => o.TotalAmount) : query.OrderBy(o => o.TotalAmount);
                        break;
                    case "status":
                        query = descending ? query.OrderByDescending(o => o.Status) : query.OrderBy(o => o.Status);
                        break;
                    default:
                        query = query.OrderByDescending(o => o.OrderDate); // Default
                        break;
                }
            }
            else
            {
                // Default sorting (most recent first)
                query = query.OrderByDescending(o => o.OrderDate);
            }

            // Apply pagination
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task UpdateAsync(Order entity)
        {
            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
