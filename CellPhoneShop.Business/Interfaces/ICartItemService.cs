using CellPhoneShop.Business.DTOs.CartItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Interfaces
{
    public interface ICartItemService
    {
        Task<CartItemDto> CreateCartItemAsync(CreateCartItemDto createCartItemDto);
        Task<IEnumerable<CartItemDto>> GetAllCartItemsAsync();
        Task<CartItemDto> GetCartItemByIdAsync(int id);
        Task UpdateAsync(int id, UpdateCartItemDto updateCartItemDto);
        Task DeleteCartItemByIdAsync(int id);
        Task<IEnumerable<CartItemDto>> GetCartItemsByCartIdAsync(int cartId);
        Task<CartItemDto> AddToCartAsync(int userId, AddToCartDto addToCartDto);
        Task RemoveFromCartAsync(int userId, int variantId);
        Task ClearCartAsync(int userId);
    }
}
