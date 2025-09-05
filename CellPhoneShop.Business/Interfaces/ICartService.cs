using CellPhoneShop.Business.DTOs.CartDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> CreateCartAsync(CreateCartDto createCartDto);
        Task<IEnumerable<CartDto>> GetAllCartsAsync();
        Task<CartDto> GetCartByIdAsync(int id);
        Task UpdateAsync(int id, UpdateCartDto updateCartDto);
        Task DeleteCartByIdAsync(int id);
        Task<CartDto> GetActiveCartByUserIdAsync(int userId);
        Task<CartDto> GetOrCreateCartForUserAsync(int userId);
    }
}
