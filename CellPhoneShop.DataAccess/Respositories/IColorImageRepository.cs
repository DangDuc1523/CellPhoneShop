using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Respositories
{
    public interface IColorImageRepository
    {
        Task<IEnumerable<ColorImage>> GetByColorIdAsync(int colorId);
        Task<ColorImage> GetByIdAsync(int id);
        Task<ColorImage> CreateAsync(ColorImage colorImage);
        Task UpdateAsync(ColorImage colorImage);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 