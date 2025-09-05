using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly CellPhoneShopContext _context;

        public DistrictRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<District>> GetAllAsync()
        {
            return await _context.Districts
                .Include(d => d.Province)
                .Include(d => d.Wards)
                .ToListAsync();
        }

        public async Task<District?> GetByIdAsync(int id)
        {
            return await _context.Districts
                .Include(d => d.Province)
                .Include(d => d.Wards)
                .FirstOrDefaultAsync(d => d.DistrictId == id);
        }

        public async Task<IEnumerable<District>> GetByProvinceIdAsync(int provinceId)
        {
            return await _context.Districts
                .Where(d => d.ProvinceId == provinceId)
                .Include(d => d.Province)
                .Include(d => d.Wards)
                .ToListAsync();
        }
    }
}
