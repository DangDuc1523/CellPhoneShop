using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Respositories
{
    public interface IProvinceRepository
    {
        Task<IEnumerable<Province>> GetAllAsync();
        Task<Province?> GetByIdAsync(int id);
    }
}
