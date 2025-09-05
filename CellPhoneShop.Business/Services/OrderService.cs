using AutoMapper;
using CellPhoneShop.Business.DTOs.OrderDtos;
using CellPhoneShop.Business.Interfaces;
using CellPhoneShop.DataAccess.Respositories.Interfaces;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;


namespace CellPhoneShop.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var newOrder = _mapper.Map<Order>(createOrderDto);
                decimal totalAmount = 0;

                foreach (var orderDetailDto in createOrderDto.OrderDetails)
                {
                    //// You might need to get variant/product information here
                    var variant = await _unitOfWork.PhoneVariants.GetByIdAsync(orderDetailDto.VariantId);

                    // Check if variant exists
                    if (variant == null)
                    {
                        throw new KeyNotFoundException($"Variant with ID {orderDetailDto.VariantId} not found.");
                    }

                    // Check if stock quantity is less than requested quantity
                    if (variant.Stock < orderDetailDto.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product with ID {orderDetailDto.VariantId}.");
                    }

                    // Decrease stock after order
                    variant.Stock -= orderDetailDto.Quantity;

                    // Decrease stock
                    await _unitOfWork.PhoneVariants.UpdateAsync(variant);

                    // Calculate total amount
                    totalAmount += orderDetailDto.Price * orderDetailDto.Quantity;

                    // Add variant id and quantity to order

                    newOrder.OrderDetails.Add(new OrderDetail
                    {
                        VariantId = orderDetailDto.VariantId,
                        Quantity = orderDetailDto.Quantity,
                    });

                }

                newOrder.TotalAmount = totalAmount;
                newOrder.OrderDate = DateTime.Now;

                await _unitOfWork.Orders.CreateAsync(newOrder);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new Exception("An error occurred while creating the order. All operations rolling back.");
            }
        }
        public async Task UpdateStatusAsync(int orderId, int status)

        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(orderId);

            if (existingOrder == null)
            {
                throw new ArgumentException($"Order with ID {orderId} not found.");
            }

            // Validate status if needed
            if (!IsValidOrderStatus(status))
            {
                throw new ArgumentException($"Invalid order status: {status}");
            }

            existingOrder.Status = status;
            await _unitOfWork.Orders.UpdateAsync(existingOrder);
            await _unitOfWork.SaveChangesAsync();

            // Use unit of work to save changes
            await transaction.CommitAsync();
        }

        // Helper method for status validation (optional)
        private bool IsValidOrderStatus(int status)
        {
            // Assuming your status values are 0-4 based on your original code
            return status >= 0 && status <= 4;
        }

        // Helper method for status validation (optional)
        
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id '{id}' not found.");

            return _mapper.Map<OrderDto>(order);
        }

        public async Task UpdateAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id '{id}' not found.");

            _mapper.Map(updateOrderDto, order);
            await _unitOfWork.Orders.UpdateAsync(order);
            await SaveChangesAsync("Failed to update order.");
        }

        public async Task DeleteOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id '{id}' not found.");

            await _unitOfWork.Orders.DeleteByIdAsync(id);
            await SaveChangesAsync("Failed to delete order.");
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
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

        public async Task<OrderSearchResultDto> SearchOrdersAsync(OrderSearchRequestDto request)
        {
            var (orders, totalCount) = await _orderRepository.SearchOrdersAsync(
                request.SearchTerm,
                request.UserId,
                request.Status,
                request.MinAmount,
                request.MaxAmount,
                request.FromDate,
                request.ToDate,
                request.PaymentMethod,
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection
            );

            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            return new OrderSearchResultDto
            {
                Items = orderDtos,
                TotalCount = totalCount,
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                HasPreviousPage = request.Page > 1,
                HasNextPage = request.Page < totalPages
            };
        }
    }
}

