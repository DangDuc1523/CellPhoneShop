using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Business.DTOs.Location;

namespace CellPhoneShop.Business.Interfaces
{
    public interface IWardService
    {
        Task<IEnumerable<WardDTO>> GetAllWardsAsync();
        Task<WardDTO?> GetWardByIdAsync(int id);
        Task<IEnumerable<WardDTO>> GetWardsByDistrictIdAsync(int districtId);
    }
}
