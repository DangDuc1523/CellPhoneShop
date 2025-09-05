using CellPhoneShop.DataAccess.Respositories.Interfaces;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly CellPhoneShopContext _context;

        public BrandRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands
                .Where(b => b.IsDeleted != true)
                .OrderBy(b => b.BrandName)
                .ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Brands
                .FirstOrDefaultAsync(b => b.BrandId == id && b.IsDeleted != true);
        }

        public async Task<Brand> CreateAsync(Brand brand)
        {
            brand.CreatedAt = DateTime.Now;
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task UpdateAsync(Brand brand)
        {
            var existingBrand = await _context.Brands.FindAsync(brand.BrandId);
            if (existingBrand == null)
                throw new KeyNotFoundException($"Brand with ID {brand.BrandId} not found");

            existingBrand.BrandName = brand.BrandName;
            existingBrand.LogoUrl = brand.LogoUrl;
            existingBrand.Description = brand.Description;
            existingBrand.ModifiedAt = DateTime.Now;
            existingBrand.ModifiedBy = brand.ModifiedBy;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {id} not found");

            brand.IsDeleted = true;
            brand.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Brands
                .AnyAsync(b => b.BrandId == id && b.IsDeleted != true);
        }

        public async Task<bool> IsBrandNameUniqueAsync(string brandName, int? excludeId = null)
        {
            var query = _context.Brands
                .Where(b => b.IsDeleted != true && b.BrandName == brandName);

            if (excludeId.HasValue)
                query = query.Where(b => b.BrandId != excludeId);

            return !await query.AnyAsync();
        }
    }
} 