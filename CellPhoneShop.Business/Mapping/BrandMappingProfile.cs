using AutoMapper;
using CellPhoneShop.API.DTOs.Brand;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Mapping
{
    public class BrandMappingProfile : Profile
    {
        public BrandMappingProfile()
        {
            CreateMap<Brand, BrandDto>();
            CreateMap<CreateBrandDto, Brand>();
            CreateMap<UpdateBrandDto, Brand>();
        }
    }
} 