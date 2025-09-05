using AutoMapper;
using CellPhoneShop.Business.DTOs.Master;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Mapping
{
    public class MasterMappingProfile : Profile
    {
        public MasterMappingProfile()
        {
            CreateMap<Master, MasterDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MasterDataId, opt => opt.MapFrom(src => src.MasterDataId))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
} 