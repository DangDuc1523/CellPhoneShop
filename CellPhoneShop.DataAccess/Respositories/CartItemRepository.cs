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
    public class CartItemRepository : ICartItemRepository
    {
        private readonly CellPhoneShopContext _context;

        public CartItemRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CartItem entity)
        {
            await _context.CartItems.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCartItemsByCartIdAsync(int cartId, int variantId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.VariantId == variantId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            return await _context.CartItems.Include(x => x.Variant).ToListAsync();
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.CartItemId == id);
            return cartItem;
        }

        public async Task<CartItem?> GetCartItemByCartAndVariantAsync(int cartId, int variantId)
        {
            return await _context.CartItems.Include(ci => ci.Variant).ThenInclude(v => v.Color).ThenInclude(v => v.Phone)
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.VariantId == variantId);
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .Include(ci => ci.Variant).ThenInclude(v => v.Color).ThenInclude(v => v.Phone)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsWithDetailsAsync(int cartId)
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .Include(ci => ci.Variant)
                .ThenInclude(ci => ci.Color)
                .ThenInclude(ci => ci.Phone)
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }

        public async Task UpdateCartItemQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int cartId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CartItem entity)
        {
            _context.CartItems.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}