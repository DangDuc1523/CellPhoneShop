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
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;

        public DistrictService(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public async Task<IEnumerable<DistrictDTO>> GetAllDistrictsAsync()
        {
            var districts = await _districtRepository.GetAllAsync();
            return districts.Select(d => new DistrictDTO
            {
                DistrictId = d.DistrictId,
                ProvinceId = d.ProvinceId,
                DistrictName = d.DistrictName
            }).ToList();
        }

        public async Task<DistrictDTO?> GetDistrictByIdAsync(int id)
        {
            var district = await _districtRepository.GetByIdAsync(id);
            return district == null ? null : new DistrictDTO
            {
                DistrictId = district.DistrictId,
                ProvinceId = district.ProvinceId,
                DistrictName = district.DistrictName
            };
        }

        public async Task<IEnumerable<DistrictDTO>> GetDistrictsByProvinceIdAsync(int provinceId)
        {
            var districts = await _districtRepository.GetByProvinceIdAsync(provinceId);
            return districts.Select(d => new DistrictDTO
            {
                DistrictId = d.DistrictId,
                ProvinceId = d.ProvinceId,
                DistrictName = d.DistrictName
            }).ToList();
        }
    }
}
