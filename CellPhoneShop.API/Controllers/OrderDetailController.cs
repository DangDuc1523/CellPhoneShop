using CellPhoneShop.Business.DTOs.OrderDetailDtos;
using CellPhoneShop.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CellPhoneShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetailDto>> CreateOrderDetail([FromBody] CreateOrderDetailDto createOrderDetailDto)
        {
            try
            {
                var orderDetail = await _orderDetailService.CreateOrderDetailAsync(createOrderDetailDto);
                return CreatedAtAction(nameof(GetOrderDetailById), new { id = orderDetail.OrderDetailId }, orderDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetAllOrderDetails()
        {
            try
            {
                var orderDetails = await _orderDetailService.GetAllOrderDetailsAsync();
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailDto>> GetOrderDetailById(int id)
        {
            try
            {
                var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
                return Ok(orderDetail);
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
        public async Task<ActionResult> UpdateOrderDetail(int id, [FromBody] UpdateOrderDetailDto updateOrderDetailDto)
        {
            try
            {
                await _orderDetailService.UpdateAsync(id, updateOrderDetailDto);
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
        public async Task<ActionResult> DeleteOrderDetail(int id)
        {
            try
            {
                await _orderDetailService.DeleteOrderDetailByIdAsync(id);
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

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderDetailsByOrderId(int orderId)
        {
            try
            {
                var orderDetails = await _orderDetailService.GetOrderDetailsByOrderIdAsync(orderId);
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}