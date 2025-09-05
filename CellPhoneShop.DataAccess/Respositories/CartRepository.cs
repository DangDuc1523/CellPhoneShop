using CellPhoneShop.DataAccess.Respositories.Interfaces;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CellPhoneShopContext _context;

        public CartRepository(CellPhoneShopContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Cart entity)
        {
            await _context.Carts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
           var cart = await _context.Orders.FindAsync(id);

            _context.Orders.Remove(cart!);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetActiveCartByUserIdAsync(int userId)
        {
            var cart = await _context.Carts.Where(c => c.UserId == userId).FirstOrDefaultAsync();
            return cart;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart> GetByIdAsync(int id)
        {
            var cart =  await _context.Carts
                .Include( c => c.CartItems )
                .FirstOrDefaultAsync( c => c.CartId == id);
            return cart;
        }

        public async Task<Cart> GetOrCreateCartForUserAsync(int userId)
        {
            // First, try to find an existing active cart for the user
            var existingCart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive == true);

            if (existingCart != null)
            {
                return existingCart;
            }

            // If no active cart exists, create a new one
            var newCart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Carts.Add(newCart);
            await _context.SaveChangesAsync();

            return newCart;
        }

        public async Task UpdateAsync(Cart entity)
        {
            _context.Carts.Update(entity);
          await  _context.SaveChangesAsync();
        }
    }
}
