using AutoMapper;
using CellPhoneShop.Business.DTOs.CartItemDtos;
using CellPhoneShop.Domain.Models;
using System.Linq;

namespace CellPhoneShop.Business.Mapping
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            // Mapping từ DTO để tạo mới CartItem
            CreateMap<CreateCartItemDto, CartItem>()
                .ForMember(dest => dest.CartItemId, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Variant, opt => opt.Ignore());

            // Mapping từ DTO khi thêm vào giỏ hàng (AddToCartDto → CartItem)
            CreateMap<AddToCartDto, CartItem>()
                .ForMember(dest => dest.CartItemId, opt => opt.Ignore())
                .ForMember(dest => dest.CartId, opt => opt.Ignore())  // Có thể giữ nếu gán ở tầng service
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Variant, opt => opt.Ignore());

            // Mapping để cập nhật CartItem
            CreateMap<UpdateCartItemDto, CartItem>()
                .ForMember(dest => dest.CartItemId, opt => opt.Ignore())
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.VariantId, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Variant, opt => opt.Ignore());

            // Mapping sang DTO để trả về dữ liệu
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.PhoneName, opt => opt.MapFrom(src =>
                    src.Variant.Color.Phone.PhoneName)) // mapping xuyên suốt Variant → Color → Phone
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src =>
                    src.Variant.Price)) // Giả định Variant có thuộc tính Price
                .ForMember(dest => dest.ColorImageUrls, opt => opt.MapFrom(src =>
                    src.Variant.Color.ColorImages.Select(img => img.ImageUrl).ToList())) // mapping gallery ảnh
                .ReverseMap() // nếu bạn muốn hỗ trợ mapping ngược
                .ForMember(dest => dest.Variant, opt => opt.Ignore()) // tránh map ngược phức tạp
                .ForMember(dest => dest.Cart, opt => opt.Ignore());
        }
    }
}
