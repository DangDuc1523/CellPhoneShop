using AutoMapper;
using CellPhoneShop.Business.DTOs;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Mapping
{
    public class ColorMappingProfile : Profile
    {
        public ColorMappingProfile()
        {
            CreateMap<Color, ColorDto>();

            CreateMap<CreateColorDto, Color>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.ColorId, opt => opt.Ignore())
                .ForMember(dest => dest.ColorImages, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneVariants, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedByNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            CreateMap<UpdateColorDto, Color>()
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ColorId, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneId, opt => opt.Ignore())
                .ForMember(dest => dest.ColorImages, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneVariants, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedByNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedByNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        }
    }
} 