using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Respositories
{
    public interface IDistrictRepository
    {
        Task<IEnumerable<District>> GetAllAsync();
        Task<District?> GetByIdAsync(int id);
        Task<IEnumerable<District>> GetByProvinceIdAsync(int provinceId);
    }
}
