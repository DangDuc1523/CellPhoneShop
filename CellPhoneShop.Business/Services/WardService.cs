using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Business.DTOs.Location;
using CellPhoneShop.Business.Interfaces;
using CellPhoneShop.DataAccess.Respositories;

namespace CellPhoneShop.Business.Services
{
    public class WardService : IWardService
    {
        private readonly IWardRepository _wardRepository;

        public WardService(IWardRepository wardRepository)
        {
            _wardRepository = wardRepository;
        }

        public async Task<IEnumerable<WardDTO>> GetAllWardsAsync()
        {
            var wards = await _wardRepository.GetAllAsync();
            return wards.Select(w => new WardDTO
            {
                WardId = w.WardId,
                DistrictId = w.DistrictId,
                WardName = w.WardName
            }).ToList();
        }

        public async Task<WardDTO?> GetWardByIdAsync(int id)
        {
            var ward = await _wardRepository.GetByIdAsync(id);
            return ward == null ? null : new WardDTO
            {
                WardId = ward.WardId,
                DistrictId = ward.DistrictId,
                WardName = ward.WardName
            };
        }

        public async Task<IEnumerable<WardDTO>> GetWardsByDistrictIdAsync(int districtId)
        {
            var wards = await _wardRepository.GetByDistrictIdAsync(districtId);
            return wards.Select(w => new WardDTO
            {
                WardId = w.WardId,
                DistrictId = w.DistrictId,
                WardName = w.WardName
            }).ToList();
        }
    }
}
