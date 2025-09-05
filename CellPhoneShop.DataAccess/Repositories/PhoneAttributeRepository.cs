using CellPhoneShop.DataAccess.Repositories.Interfaces;
using CellPhoneShop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Repositories
{
    public class PhoneAttributeRepository : IPhoneAttributeRepository
    {
        private readonly CellPhoneShopContext _context;
        public PhoneAttributeRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhoneAttribute>> GetAllAsync()
        {
            return await _context.PhoneAttributes.Where(x => x.IsDeleted != true).ToListAsync();
        }

        public async Task<PhoneAttribute?> GetByIdAsync(int id)
        {
            return await _context.PhoneAttributes.FirstOrDefaultAsync(x => x.AttributeId == id && x.IsDeleted != true);
        }

        public async Task<PhoneAttribute> CreateAsync(PhoneAttribute phoneAttribute)
        {
            _context.PhoneAttributes.Add(phoneAttribute);
            await _context.SaveChangesAsync();
            return phoneAttribute;
        }

        public async Task<PhoneAttribute> UpdateAsync(PhoneAttribute phoneAttribute)
        {
            _context.PhoneAttributes.Update(phoneAttribute);
            await _context.SaveChangesAsync();
            return phoneAttribute;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.PhoneAttributes.FindAsync(id);
            if (entity == null || entity.IsDeleted == true)
                return false;
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;
            _context.PhoneAttributes.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PhoneAttributes.AnyAsync(x => x.AttributeId == id && x.IsDeleted != true);
        }
    }
} 