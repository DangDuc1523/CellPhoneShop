using AutoMapper;
using CellPhoneShop.Business.DTOs.CartDtos;
using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Mapping
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CreateCartDto, Cart>()
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => new List<CartItem>()))
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdateCartDto, Cart>()
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CartItems, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<CartDto, Cart>().ReverseMap();
        }
    }
}
