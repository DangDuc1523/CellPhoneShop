using CellPhoneShop.Business.DTOs.CartItemDtos;
using CellPhoneShop.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CellPhoneShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> CreateCartItem([FromBody] CreateCartItemDto createCartItemDto)
        {
            try
            {
                var cartItem = await _cartItemService.CreateCartItemAsync(createCartItemDto);
                return CreatedAtAction(nameof(GetCartItemById), new { id = cartItem.CartItemId }, cartItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetAllCartItems()
        {
            try
            {
                var cartItems = await _cartItemService.GetAllCartItemsAsync();
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItemDto>> GetCartItemById(int id)
        {
            try
            {
                var cartItem = await _cartItemService.GetCartItemByIdAsync(id);
                return Ok(cartItem);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCartItem(int id, [FromBody] UpdateCartItemDto updateCartItemDto)
        {
            try
            {
                await _cartItemService.UpdateAsync(id, updateCartItemDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            try
            {
                await _cartItemService.DeleteCartItemByIdAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("cart/{cartId}")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItemsByCartId(int cartId)
        {
            try
            {
                var cartItems = await _cartItemService.GetCartItemsByCartIdAsync(cartId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("add-to-cart")]
        public async Task<ActionResult<CartItemDto>> AddToCart(int userId, [FromBody] AddToCartDto addToCartDto)
        {
            try
            {
                var cartItem = await _cartItemService.AddToCartAsync(userId, addToCartDto);
                return Ok(cartItem);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("remove-from-cart")]
        public async Task<ActionResult> RemoveFromCart(int userId, [FromQuery] int variantId)
        {
            try
            {
                await _cartItemService.RemoveFromCartAsync(userId, variantId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("clear-cart/{userId}")]
        public async Task<ActionResult> ClearCart(int userId)
        {
            try
            {
                await _cartItemService.ClearCartAsync(userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}