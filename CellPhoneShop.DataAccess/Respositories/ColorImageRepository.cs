using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class ColorImageRepository : IColorImageRepository
    {
        private readonly CellPhoneShopContext _context;

        public ColorImageRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ColorImage>> GetByColorIdAsync(int colorId)
        {
            return await _context.ColorImages
                .Where(ci => ci.IsDeleted != true && ci.ColorId == colorId)
                .ToListAsync();
        }

        public async Task<ColorImage> GetByIdAsync(int id)
        {
            return await _context.ColorImages
                .FirstOrDefaultAsync(ci => ci.ImageId == id && ci.IsDeleted != true);
        }

        public async Task<ColorImage> CreateAsync(ColorImage colorImage)
        {
            colorImage.CreatedAt = DateTime.UtcNow;
            colorImage.IsDeleted = false;
            _context.ColorImages.Add(colorImage);
            await _context.SaveChangesAsync();
            return colorImage;
        }

        public async Task UpdateAsync(ColorImage colorImage)
        {
            var existingImage = await _context.ColorImages.FindAsync(colorImage.ImageId);
            if (existingImage == null)
                throw new KeyNotFoundException($"ColorImage with ID {colorImage.ImageId} not found");

            colorImage.ModifiedAt = DateTime.UtcNow;
            _context.Entry(existingImage).CurrentValues.SetValues(colorImage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var colorImage = await _context.ColorImages.FindAsync(id);
            if (colorImage == null)
                throw new KeyNotFoundException($"ColorImage with ID {id} not found");

            colorImage.IsDeleted = true;
            colorImage.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ColorImages
                .AnyAsync(ci => ci.ImageId == id && ci.IsDeleted != true);
        }
    }
} 