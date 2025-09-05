using AutoMapper;
using CellPhoneShop.Business.DTOs.CartItemDtos;
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

    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartItemDto> CreateCartItemAsync(CreateCartItemDto createCartItemDto)
        {
            var cartItem = _mapper.Map<CartItem>(createCartItemDto);
            await _unitOfWork.CartItems.CreateAsync(cartItem);
            await SaveChangesAsync("Failed to create cart item.");

            return _mapper.Map<CartItemDto>(cartItem);
        }

        public async Task<IEnumerable<CartItemDto>> GetAllCartItemsAsync()
        {
            var cartItems = await _unitOfWork.CartItems.GetAllAsync();
            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }

        public async Task<CartItemDto> GetCartItemByIdAsync(int id)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (cartItem == null)
                throw new KeyNotFoundException($"Cart item with id '{id}' not found.");

            return _mapper.Map<CartItemDto>(cartItem);
        }

        public async Task UpdateAsync(int id, UpdateCartItemDto updateCartItemDto)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (cartItem == null)
                throw new KeyNotFoundException($"Cart item with id '{id}' not found.");

            _mapper.Map(updateCartItemDto, cartItem);
            await _unitOfWork.CartItems.UpdateAsync(cartItem);
            await SaveChangesAsync("Failed to update cart item.");
        }

        public async Task DeleteCartItemByIdAsync(int id)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (cartItem == null)
                throw new KeyNotFoundException($"Cart item with id '{id}' not found.");

            await _unitOfWork.CartItems.DeleteByIdAsync(id);
            await SaveChangesAsync("Failed to delete cart item.");
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsByCartIdAsync(int cartId)
        {
            var cartItems = await _unitOfWork.CartItems.GetCartItemsByCartIdAsync(cartId);
            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }

        public async Task<CartItemDto> AddToCartAsync(int userId, AddToCartDto addToCartDto)
        {
            // Get or create cart for user
            var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, IsActive = true, CreatedAt = DateTime.Now };
                await _unitOfWork.Carts.CreateAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            // Check if item already exists in cart
            var existingItem = await _unitOfWork.CartItems.GetCartItemByCartAndVariantAsync(cart.CartId, addToCartDto.VariantId);

            if (existingItem != null)
            {
                // Update quantity if item exists
                existingItem.Quantity += addToCartDto.Quantity;
                await _unitOfWork.CartItems.UpdateAsync(existingItem);
                await SaveChangesAsync("Failed to update cart item quantity.");
                return _mapper.Map<CartItemDto>(existingItem);
            }
            else
            {
                // Create new cart item
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    VariantId = addToCartDto.VariantId,
                    Quantity = addToCartDto.Quantity
                };

                await _unitOfWork.CartItems.CreateAsync(cartItem);
                await SaveChangesAsync("Failed to add item to cart.");
                return _mapper.Map<CartItemDto>(cartItem);
            }
        }

        public async Task RemoveFromCartAsync(int userId, int variantId)
        {
            var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException($"No active cart found for user '{userId}'.");

            var cartItem = await _unitOfWork.CartItems.GetCartItemByCartAndVariantAsync(cart.CartId, variantId);
            if (cartItem == null)
                throw new KeyNotFoundException($"Cart item not found.");

            await _unitOfWork.CartItems.DeleteByIdAsync(cartItem.CartItemId);
            await SaveChangesAsync("Failed to remove item from cart.");
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException($"No active cart found for user '{userId}'.");

            await _unitOfWork.CartItems.DeleteByIdAsync(cart.CartId);
            await SaveChangesAsync("Failed to clear cart.");
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

