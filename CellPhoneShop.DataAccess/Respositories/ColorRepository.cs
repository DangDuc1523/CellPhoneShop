using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly CellPhoneShopContext _context;

        public ColorRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Color>> GetByPhoneIdAsync(int phoneId)
        {
            return await _context.Colors
                .Where(c => c.IsDeleted != true && c.PhoneId == phoneId)
                .ToListAsync();
        }

        public async Task<Color> GetByIdAsync(int id)
        {
            return await _context.Colors
                .FirstOrDefaultAsync(c => c.ColorId == id && c.IsDeleted != true);
        }

        public async Task<Color> CreateAsync(Color color)
        {
            color.CreatedAt = DateTime.UtcNow;
            color.IsDeleted = false;
            _context.Colors.Add(color);
            await _context.SaveChangesAsync();
            return color;
        }

        public async Task UpdateAsync(Color color)
        {
            var existingColor = await _context.Colors.FindAsync(color.ColorId);
            if (existingColor == null)
                throw new KeyNotFoundException($"Color with ID {color.ColorId} not found");

            color.ModifiedAt = DateTime.UtcNow;
            _context.Entry(existingColor).CurrentValues.SetValues(color);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
                throw new KeyNotFoundException($"Color with ID {id} not found");

            color.IsDeleted = true;
            color.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Colors
                .AnyAsync(c => c.ColorId == id && c.IsDeleted != true);
        }
    }
} 