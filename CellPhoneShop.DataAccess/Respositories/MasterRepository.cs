using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class MasterRepository : IMasterRepository
    {
        private readonly CellPhoneShopContext _context;

        public MasterRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Master>> GetByCategoryAsync(string category)
        {
            return await _context.Masters
                .Where(m => m.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Master>> GetByCategoryAndMasterDataIdAsync(string category, int masterDataId)
        {
            return await _context.Masters
                .Where(m => m.Category == category && m.MasterDataId == masterDataId)
                .ToListAsync();
        }
    }
}
