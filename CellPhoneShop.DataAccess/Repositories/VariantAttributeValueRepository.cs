using CellPhoneShop.DataAccess.Repositories.Interfaces;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Repositories
{
    public class VariantAttributeValueRepository : IVariantAttributeValueRepository
    {
        private readonly CellPhoneShopContext _context;

        public VariantAttributeValueRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VariantAttributeValue>> GetAllAsync()
        {
            return await _context.VariantAttributeValues
                .Where(x => x.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<IEnumerable<VariantAttributeValue>> GetByAttributeIdAsync(int attributeId)
        {
            return await _context.VariantAttributeValues
                .Where(x => x.VariantAttributeId == attributeId && x.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<VariantAttributeValue?> GetByIdAsync(int id)
        {
            return await _context.VariantAttributeValues
                .FirstOrDefaultAsync(x => x.ValueId == id && x.IsDeleted != true);
        }

        public async Task<VariantAttributeValue> CreateAsync(VariantAttributeValue value)
        {
            value.CreatedAt = DateTime.UtcNow;
            value.IsDeleted = false;
            _context.VariantAttributeValues.Add(value);
            await _context.SaveChangesAsync();
            return value;
        }

        public async Task<VariantAttributeValue> UpdateAsync(VariantAttributeValue value)
        {
            var existing = await _context.VariantAttributeValues.FindAsync(value.ValueId);
            if (existing == null)
                throw new KeyNotFoundException($"VariantAttributeValue with ID {value.ValueId} not found");

            value.ModifiedAt = DateTime.UtcNow;
            _context.Entry(existing).CurrentValues.SetValues(value);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var value = await _context.VariantAttributeValues.FindAsync(id);
            if (value == null)
                return false;

            value.IsDeleted = true;
            value.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 