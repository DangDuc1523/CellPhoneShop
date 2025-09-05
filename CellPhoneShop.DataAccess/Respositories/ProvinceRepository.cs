using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly CellPhoneShopContext _context;

        public ProvinceRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Province>> GetAllAsync()
        {
            return await _context.Provinces
                .Include(p => p.Districts)
                .ToListAsync();
        }

        public async Task<Province?> GetByIdAsync(int id)
        {
            return await _context.Provinces
                .Include(p => p.Districts)
                .FirstOrDefaultAsync(p => p.ProvinceId == id);
        }
    }
}
