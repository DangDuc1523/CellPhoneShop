using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Business.DTOs.Location;

namespace CellPhoneShop.Business.Interfaces
{
    public interface IDistrictService
    {
        Task<IEnumerable<DistrictDTO>> GetAllDistrictsAsync();
        Task<DistrictDTO?> GetDistrictByIdAsync(int id);
        Task<IEnumerable<DistrictDTO>> GetDistrictsByProvinceIdAsync(int provinceId);
    }
}
