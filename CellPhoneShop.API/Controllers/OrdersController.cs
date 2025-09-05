using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CellPhoneShop.Domain.Models;
using CellPhoneShop.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CellPhoneShop.Business.DTOs.OrderDtos;
using System.Security.Claims;
using CellPhoneShop.Business.Types;

namespace CellPhoneShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger; // Fixed logger type

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersAsync()
        {
            try
            {
                // Log authentication information
                var authHeader = Request.Headers["Authorization"].ToString();
                _logger.LogInformation("Authorization Header: {AuthHeader}", authHeader);

                // Log user claims
                var claims = User.Claims.Select(c => new { c.Type, c.Value });
                _logger.LogInformation("User Claims: {@Claims}", claims);

                // Log user identity
                _logger.LogInformation("IsAuthenticated: {IsAuthenticated}", User.Identity?.IsAuthenticated);
                _logger.LogInformation("AuthenticationType: {AuthType}", User.Identity?.AuthenticationType);
                _logger.LogInformation("Name: {Name}", User.Identity?.Name);

                // Log roles
                var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                _logger.LogInformation("User Roles: {@Roles}", roles);

                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders); // Direct return without wrapper
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting orders");
                return StatusCode(500, "An error occurred while getting the orders");
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null)
                {
                    return NotFound($"Order with ID {id} not found.");
                }

                return Ok(order); // Direct return without wrapper
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting order {OrderId}", id);
                return StatusCode(500, "An error occurred while getting the order");
            }
        }


        // GET: api/Orders/5
        [HttpGet("OrderByUserId/{id}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderByUserId(int id)
        {
            try
            {
                var order = await _orderService.GetOrdersByUserIdAsync(id);

                if (order == null)
                {
                    return NotFound($"Order with ID {id} not found.");
                }

                return Ok(order); // Direct return without wrapper
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting order {OrderId}", id);
                return StatusCode(500, "An error occurred while getting the order");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto newOrder)
        {
            await _orderService.CreateOrderAsync(newOrder);

            return NoContent();
        }

        // PUT: api/Orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto orderToUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _orderService.UpdateAsync(id, orderToUpdate);
                return NoContent(); // Standard REST response for successful update
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating order {OrderId}", id);
                return StatusCode(500, "An error occurred while updating the order");
            }
        }

        // PUT: api/Orders/{id}/status - Add this specific endpoint for status updates
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto statusUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _orderService.UpdateStatusAsync(id, statusUpdate.Status);
                return NoContent(); // Standard REST response for successful update
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating order status {OrderId}", id);
                return StatusCode(500, "An error occurred while updating the order status");
            }
        }

        // DELETE: api/Orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderById(int id)
        {
            try
            {
                await _orderService.DeleteOrderByIdAsync(id);
                return NoContent(); // Standard REST response for successful deletion
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting order {OrderId}", id);
                return StatusCode(500, "An error occurred while deleting the order");
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<ActionResult<OrderSearchResultDto>> SearchOrders([FromQuery] OrderSearchRequestDto request)
        {
            try
            {
                var result = await _orderService.SearchOrdersAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching orders");
                return StatusCode(500, "An error occurred while searching the orders");
            }
        }
    }
}

// Add this DTO for status updates
public class UpdateOrderStatusDto
{
    public int Status { get; set; }
}