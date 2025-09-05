using AutoMapper;
using CellPhoneShop.Business.DTOs.OrderDtos;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>()
               .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
               .ForMember(dest => dest.OrderId, opt => opt.Ignore())
               .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdateOrderDto, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

            CreateMap<OrderDto, Order>().ReverseMap();
        }
    }
}
