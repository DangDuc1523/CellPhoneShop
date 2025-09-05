using AutoMapper;
using CellPhoneShop.Business.DTOs;
using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Mapping
{
    public class UserMappingProfile : Profile
    {

        public UserMappingProfile()
        {
            CreateMap<UserProfileDto, UserAccount>();
            CreateMap<UserAccount, UserProfileDto>();

        }
            
    }
}
