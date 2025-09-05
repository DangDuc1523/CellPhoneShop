using CellPhoneShop.Business.DTOs.OrderDtos;
using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Interfaces
{
    public interface IOrderService
    {
       Task CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task UpdateAsync(int id, UpdateOrderDto updateOrderDto);
        Task DeleteOrderByIdAsync(int id);
        Task UpdateStatusAsync(int orderId, int status);
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<OrderSearchResultDto> SearchOrdersAsync(OrderSearchRequestDto request);
    }

}
