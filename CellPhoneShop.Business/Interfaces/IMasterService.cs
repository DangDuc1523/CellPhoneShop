using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Business.DTOs.Master;

namespace CellPhoneShop.Business.Interfaces
{
    public interface IMasterService
    {
        Task<IEnumerable<MasterDTO>> GetByCategoryAsync(string category);
        Task<IEnumerable<MasterDTO>> GetByCategoryAndMasterDataIdAsync(string category, int masterDataId);
    }
}
