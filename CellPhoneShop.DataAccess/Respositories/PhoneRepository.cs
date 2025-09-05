using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly CellPhoneShopContext _context;

        public PhoneRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Phone>> GetAllAsync()
        {
            return await _context.Phones
                .Include(p => p.Brand)
                .Where(p => p.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<Phone> GetByIdAsync(int id)
        {
            return await _context.Phones
                .Include(p => p.Brand)
                .Include(p => p.PhoneAttributeMappings)
                    .ThenInclude(pam => pam.Attribute)
                .FirstOrDefaultAsync(p => p.PhoneId == id && p.IsDeleted != true);
        }

        public async Task<IEnumerable<Phone>> SearchAsync(
            string? searchTerm,
            int? brandId,
            decimal? minPrice,
            decimal? maxPrice,
            string? ram,
            string? os,
            int page = 1,
            int pageSize = 10,
            string? sortBy = null,
            bool isAscending = true)
        {
            var query = _context.Phones
                .Include(p => p.Brand)
                .Where(p => p.IsDeleted != true);

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => 
                    p.PhoneName.Contains(searchTerm) || 
                    (p.Description != null && p.Description.Contains(searchTerm)));
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice <= maxPrice);
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "name" => isAscending ? query.OrderBy(p => p.PhoneName) : query.OrderByDescending(p => p.PhoneName),
                    "price" => isAscending ? query.OrderBy(p => p.BasePrice) : query.OrderByDescending(p => p.BasePrice),
                    "brand" => isAscending ? query.OrderBy(p => p.Brand.BrandName) : query.OrderByDescending(p => p.Brand.BrandName),
                    _ => query.OrderBy(p => p.PhoneId)
                };
            }

            // Apply pagination
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(
            string? searchTerm = null,
            int? brandId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? ram = null,
            string? os = null)
        {
            var query = _context.Phones.Where(p => p.IsDeleted != true);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => 
                    p.PhoneName.Contains(searchTerm) || 
                    (p.Description != null && p.Description.Contains(searchTerm)));
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice <= maxPrice);
            }

            return await query.CountAsync();
        }

        public async Task<Phone> CreateAsync(Phone phone)
        {
            phone.CreatedAt = DateTime.UtcNow;
            phone.IsDeleted = false;
            _context.Phones.Add(phone);
            await _context.SaveChangesAsync();
            return phone;
        }

        public async Task UpdateAsync(Phone phone)
        {
            var existingPhone = await _context.Phones.FindAsync(phone.PhoneId);
            if (existingPhone == null)
                throw new KeyNotFoundException($"Phone with ID {phone.PhoneId} not found");

            phone.ModifiedAt = DateTime.UtcNow;
            _context.Entry(existingPhone).CurrentValues.SetValues(phone);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone == null)
                throw new KeyNotFoundException($"Phone with ID {id} not found");

            phone.IsDeleted = true;
            phone.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Phones
                .AnyAsync(p => p.PhoneId == id && p.IsDeleted != true);
        }
    }
} 