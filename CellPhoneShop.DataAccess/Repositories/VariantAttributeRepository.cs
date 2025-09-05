using CellPhoneShop.DataAccess.Repositories.Interfaces;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Repositories
{
    public class VariantAttributeRepository : IVariantAttributeRepository
    {
        private readonly CellPhoneShopContext _context;

        public VariantAttributeRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VariantAttribute>> GetAllAsync()
        {
            return await _context.VariantAttributes
                .Where(x => x.IsDeleted != true)
                .Include(x => x.VariantAttributeValues.Where(v => v.IsDeleted != true))
                .ToListAsync();
        }

        public async Task<VariantAttribute?> GetByIdAsync(int id)
        {
            return await _context.VariantAttributes
                .Include(x => x.VariantAttributeValues.Where(v => v.IsDeleted != true))
                .FirstOrDefaultAsync(x => x.VariantAttributeId == id && x.IsDeleted != true);
        }

        public async Task<VariantAttribute> CreateAsync(VariantAttribute variantAttribute)
        {
            variantAttribute.CreatedAt = DateTime.UtcNow;
            variantAttribute.IsDeleted = false;
            _context.VariantAttributes.Add(variantAttribute);
            await _context.SaveChangesAsync();
            return variantAttribute;
        }

        public async Task<VariantAttribute> UpdateAsync(VariantAttribute variantAttribute)
        {
            var existing = await _context.VariantAttributes.FindAsync(variantAttribute.VariantAttributeId);
            if (existing == null)
                throw new KeyNotFoundException($"VariantAttribute with ID {variantAttribute.VariantAttributeId} not found");

            variantAttribute.ModifiedAt = DateTime.UtcNow;
            _context.Entry(existing).CurrentValues.SetValues(variantAttribute);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var variantAttribute = await _context.VariantAttributes.FindAsync(id);
            if (variantAttribute == null)
                return false;

            variantAttribute.IsDeleted = true;
            variantAttribute.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 