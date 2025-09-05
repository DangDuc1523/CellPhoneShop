using CellPhoneShop.Business.DTOs.CartDtos;
using CellPhoneShop.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CellPhoneShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateCart([FromBody] CreateCartDto createCartDto)
        {
            try
            {
                var cart = await _cartService.CreateCartAsync(createCartDto);
                return CreatedAtAction(nameof(GetCartById), new { id = cart.CartId }, cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetAllCarts()
        {
            try
            {
                var carts = await _cartService.GetAllCartsAsync();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartDto>> GetCartById(int id)
        {
            try
            {
                var cart = await _cartService.GetCartByIdAsync(id);
                return Ok(cart);
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
        public async Task<ActionResult> UpdateCart(int id, [FromBody] UpdateCartDto updateCartDto)
        {
            try
            {
                await _cartService.UpdateAsync(id, updateCartDto);
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

       

        [HttpGet("user/{userId}/active")]
        public async Task<ActionResult<CartDto>> GetActiveCartByUserId(int userId)
        {
            try
            {
                var cart = await _cartService.GetActiveCartByUserIdAsync(userId);
                return Ok(cart);
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

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CartDto>> GetOrCreateCartForUser(int userId)
        {
            try
            {
                var cart = await _cartService.GetOrCreateCartForUserAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}