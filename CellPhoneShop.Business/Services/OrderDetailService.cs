using AutoMapper;
using CellPhoneShop.Business.DTOs.OrderDetailDtos;
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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderDetailDto> CreateOrderDetailAsync(CreateOrderDetailDto createOrderDetailDto)
        {
            var orderDetail = _mapper.Map<OrderDetail>(createOrderDetailDto);
            await _unitOfWork.OrderDetail.CreateAsync(orderDetail);
            await SaveChangesAsync("Failed to create order detail.");

            return _mapper.Map<OrderDetailDto>(orderDetail);
        }

        public async Task<IEnumerable<OrderDetailDto>> GetAllOrderDetailsAsync()
        {
            var orderDetails = await _unitOfWork.OrderDetail.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDetailDto>>(orderDetails);
        }

        public async Task<OrderDetailDto> GetOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetail.GetByIdAsync(id);
            if (orderDetail == null)
                throw new KeyNotFoundException($"Order detail with id '{id}' not found.");

            return _mapper.Map<OrderDetailDto>(orderDetail);
        }

        public async Task UpdateAsync(int id, UpdateOrderDetailDto updateOrderDetailDto)
        {
            var orderDetail = await _unitOfWork.OrderDetail.GetByIdAsync(id);
            if (orderDetail == null)
                throw new KeyNotFoundException($"Order detail with id '{id}' not found.");

            _mapper.Map(updateOrderDetailDto, orderDetail);
            await _unitOfWork.OrderDetail.UpdateAsync(orderDetail);
            await SaveChangesAsync("Failed to update order detail.");
        }

        public async Task DeleteOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetail.GetByIdAsync(id);
            if (orderDetail == null)
                throw new KeyNotFoundException($"Order detail with id '{id}' not found.");

            await _unitOfWork.OrderDetail.DeleteByIdAsync(id);
            await SaveChangesAsync("Failed to delete order detail.");
        }

        public async Task<IEnumerable<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var orderDetails = await _unitOfWork.OrderDetail.GetOrderDetailsByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderDetailDto>>(orderDetails);
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
