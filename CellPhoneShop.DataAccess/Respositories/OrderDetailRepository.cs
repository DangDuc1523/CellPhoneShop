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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly CellPhoneShopContext _context;

        public OrderDetailRepository(CellPhoneShopContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(OrderDetail entity)
        {
            await _context.OrderDetails.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var orderProduct = await _context.OrderDetails.FindAsync(id);

            // Remove order
            _context.OrderDetails.Remove(orderProduct!);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _context.OrderDetails.ToListAsync(); 
        }

        public async Task<OrderDetail> GetByIdAsync(int id)
        {
            var orderProduct = await _context.OrderDetails.FirstOrDefaultAsync(o => o.OrderId == id);

            return orderProduct!;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
           var orderDetails = await _context.OrderDetails.Where(o => o.OrderId == orderId).ToListAsync();
            return orderDetails;
        }

        public async  Task UpdateAsync(OrderDetail entity)
        {
            _context.OrderDetails.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
