using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Business.DTOs.Location;

namespace CellPhoneShop.Business.Interfaces
{
    public interface IProvinceService
    {
        Task<IEnumerable<ProvinceDTO>> GetAllProvincesAsync();
        Task<ProvinceDTO?> GetProvinceByIdAsync(int id);
    }
}
