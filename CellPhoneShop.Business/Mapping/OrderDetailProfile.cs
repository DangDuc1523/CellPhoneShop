using AutoMapper;
using CellPhoneShop.Business.DTOs.OrderDetailDtos;
using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Mapping
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<CreateOrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.OrderDetailId, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Variant, opt => opt.Ignore());

            CreateMap<UpdateOrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.OrderDetailId, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.VariantId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Variant, opt => opt.Ignore());

            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
        }
    }
}
