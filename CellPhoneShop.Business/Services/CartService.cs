using AutoMapper;
using CellPhoneShop.Business.DTOs.CartDtos;
using CellPhoneShop.Business.Interfaces;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartDto> CreateCartAsync(CreateCartDto createCartDto)
        {
            var cart = _mapper.Map<Cart>(createCartDto);
            await _unitOfWork.Carts.CreateAsync(cart);
            await SaveChangesAsync("Failed to create cart.");

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<IEnumerable<CartDto>> GetAllCartsAsync()
        {
            var carts = await _unitOfWork.Carts.GetAllAsync();
            return _mapper.Map<IEnumerable<CartDto>>(carts);
        }

        public async Task<CartDto> GetCartByIdAsync(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with id '{id}' not found.");

            return _mapper.Map<CartDto>(cart);
        }

        public async Task UpdateAsync(int id, UpdateCartDto updateCartDto)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with id '{id}' not found.");

            _mapper.Map(updateCartDto, cart);
            await _unitOfWork.Carts.UpdateAsync(cart);
            await SaveChangesAsync("Failed to update cart.");
        }

        public async Task DeleteCartByIdAsync(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with id '{id}' not found.");

            await _unitOfWork.Carts.DeleteByIdAsync(id);
            await SaveChangesAsync("Failed to delete cart.");
        }

        public async Task<CartDto> GetActiveCartByUserIdAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException($"No active cart found for user '{userId}'.");

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetOrCreateCartForUserAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);

            if (cart == null)
            {
                var createCartDto = new CreateCartDto { UserId = userId };
                return await CreateCartAsync(createCartDto);
            }

            return _mapper.Map<CartDto>(cart);
        }

        private async Task SaveChangesAsync(string errorMessage)
        {
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{errorMessage}. Details: {ex.Message}");
            }
        }
    }

}
