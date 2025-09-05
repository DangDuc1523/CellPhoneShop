using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Respositories
{
    public interface IMasterRepository
    {
        Task<IEnumerable<Master>> GetByCategoryAsync(string category);
        Task<IEnumerable<Master>> GetByCategoryAndMasterDataIdAsync(string category, int masterDataId);
    }
}
