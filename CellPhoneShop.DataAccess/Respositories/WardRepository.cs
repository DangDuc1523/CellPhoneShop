using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class WardRepository : IWardRepository
    {
        private readonly CellPhoneShopContext _context;

        public WardRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ward>> GetAllAsync()
        {
            return await _context.Wards
                .Include(w => w.District)
                .ToListAsync();
        }

        public async Task<Ward?> GetByIdAsync(int id)
        {
            return await _context.Wards
                .Include(w => w.District)
                .FirstOrDefaultAsync(w => w.WardId == id);
        }

        public async Task<IEnumerable<Ward>> GetByDistrictIdAsync(int districtId)
        {
            return await _context.Wards
                .Where(w => w.DistrictId == districtId)
                .Include(w => w.District)
                .ToListAsync();
        }
    }
}
