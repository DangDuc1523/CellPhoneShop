using AutoMapper;
using CellPhoneShop.Business.DTOs.Location;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Mapping
{
    public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile()
        {
            CreateMap<Province, ProvinceDTO>()
                .ForMember(dest => dest.ProvinceId, opt => opt.MapFrom(src => src.ProvinceId))
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.ProvinceName));

            CreateMap<District, DistrictDTO>()
                .ForMember(dest => dest.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
                .ForMember(dest => dest.ProvinceId, opt => opt.MapFrom(src => src.ProvinceId))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.DistrictName));

            CreateMap<Ward, WardDTO>()
                .ForMember(dest => dest.WardId, opt => opt.MapFrom(src => src.WardId))
                .ForMember(dest => dest.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
                .ForMember(dest => dest.WardName, opt => opt.MapFrom(src => src.WardName));
        }
    }
} 