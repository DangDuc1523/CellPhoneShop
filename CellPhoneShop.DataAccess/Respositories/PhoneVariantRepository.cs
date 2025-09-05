using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class PhoneVariantRepository : IPhoneVariantRepository
    {
        private readonly CellPhoneShopContext _context;

        public PhoneVariantRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhoneVariant>> GetByPhoneIdAsync(int phoneId)
        {
            return await _context.PhoneVariants
                .Where(pv => pv.IsDeleted != true && pv.PhoneId == phoneId)
                .Include(pv => pv.Color)
                .ToListAsync();
        }

        public async Task<IEnumerable<PhoneVariant>> GetByColorIdAsync(int colorId)
        {
            return await _context.PhoneVariants
                .Where(pv => pv.IsDeleted != true && pv.ColorId == colorId)
                .Include(pv => pv.Phone)
                .ToListAsync();
        }

        public async Task<PhoneVariant> GetByIdAsync(int id)
        {
            return await _context.PhoneVariants
                .Include(pv => pv.Phone)
                .Include(pv => pv.Color)
                .FirstOrDefaultAsync(pv => pv.VariantId == id && pv.IsDeleted != true);
        }

        public async Task<PhoneVariant> CreateAsync(PhoneVariant phoneVariant)
        {
            phoneVariant.CreatedAt = DateTime.UtcNow;
            phoneVariant.IsDeleted = false;
            _context.PhoneVariants.Add(phoneVariant);
            await _context.SaveChangesAsync();
            return phoneVariant;
        }

        public async Task UpdateAsync(PhoneVariant phoneVariant)
        {
            var existingVariant = await _context.PhoneVariants.FindAsync(phoneVariant.VariantId);
            if (existingVariant == null)
                throw new KeyNotFoundException($"PhoneVariant with ID {phoneVariant.VariantId} not found");

            phoneVariant.ModifiedAt = DateTime.UtcNow;
            _context.Entry(existingVariant).CurrentValues.SetValues(phoneVariant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var phoneVariant = await _context.PhoneVariants.FindAsync(id);
            if (phoneVariant == null)
                throw new KeyNotFoundException($"PhoneVariant with ID {id} not found");

            phoneVariant.IsDeleted = true;
            phoneVariant.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PhoneVariants
                .AnyAsync(pv => pv.VariantId == id && pv.IsDeleted != true);
        }

        public async Task<bool> IsSkuUniqueAsync(string sku, int? excludeVariantId = null)
        {
            if (string.IsNullOrEmpty(sku))
                return true;

            var query = _context.PhoneVariants
                .Where(pv => pv.IsDeleted != true && pv.Sku == sku);

            if (excludeVariantId.HasValue)
                query = query.Where(pv => pv.VariantId != excludeVariantId);

            return !await query.AnyAsync();
        }
    }
} 