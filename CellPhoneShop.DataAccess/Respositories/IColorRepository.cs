using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Respositories
{
    public interface IColorRepository
    {
        Task<IEnumerable<Color>> GetByPhoneIdAsync(int phoneId);
        Task<Color> GetByIdAsync(int id);
        Task<Color> CreateAsync(Color color);
        Task UpdateAsync(Color color);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 