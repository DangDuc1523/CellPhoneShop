using CellPhoneShop.DataAccess.Repositories.Interfaces;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Repositories
{
    public class PhoneAttributeMappingRepository : IPhoneAttributeMappingRepository
    {
        private readonly CellPhoneShopContext _context;
        public PhoneAttributeMappingRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<List<PhoneAttributeMapping>> GetByPhoneIdAsync(int phoneId)
        {
            return await _context.PhoneAttributeMappings.Where(x => x.PhoneId == phoneId && x.IsDeleted != true).ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<PhoneAttributeMapping> mappings)
        {
            await _context.PhoneAttributeMappings.AddRangeAsync(mappings);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByPhoneIdAsync(int phoneId)
        {
            var mappings = await _context.PhoneAttributeMappings
                .Where(x => x.PhoneId == phoneId)
                .ToListAsync();

            _context.PhoneAttributeMappings.RemoveRange(mappings);
            await _context.SaveChangesAsync();

            // Clear change tracker to avoid tracking conflicts
            _context.ChangeTracker.Clear();
        }
    }
} 