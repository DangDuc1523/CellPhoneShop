using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Respositories
{
    public interface IWardRepository
    {
        Task<IEnumerable<Ward>> GetAllAsync();
        Task<Ward?> GetByIdAsync(int id);
        Task<IEnumerable<Ward>> GetByDistrictIdAsync(int districtId);
    }
}
