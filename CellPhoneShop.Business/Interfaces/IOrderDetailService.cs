using CellPhoneShop.Business.DTOs.OrderDetailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Interfaces
{
    public interface IOrderDetailService
    {
        Task<OrderDetailDto> CreateOrderDetailAsync(CreateOrderDetailDto createOrderDetailDto);
        Task<IEnumerable<OrderDetailDto>> GetAllOrderDetailsAsync();
        Task<OrderDetailDto> GetOrderDetailByIdAsync(int id);
        Task UpdateAsync(int id, UpdateOrderDetailDto updateOrderDetailDto);
        Task DeleteOrderDetailByIdAsync(int id);
        Task<IEnumerable<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId);
    }
}
