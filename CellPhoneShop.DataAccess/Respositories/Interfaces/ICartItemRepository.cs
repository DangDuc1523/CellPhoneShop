using CellPhoneShop.Domain.Models;
using CellPhoneShop.DataAccess.Respositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Respositories.Interfaces
{
    public interface  ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task<CartItem?> GetCartItemByCartAndVariantAsync(int cartId, int variantId);
        Task DeleteCartItemsByCartIdAsync(int cartId, int variantId);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
    }
}
