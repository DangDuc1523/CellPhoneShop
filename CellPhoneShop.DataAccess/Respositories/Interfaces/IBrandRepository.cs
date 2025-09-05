using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Respositories.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(int id);
        Task<Brand> CreateAsync(Brand brand);
        Task UpdateAsync(Brand brand);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsBrandNameUniqueAsync(string brandName, int? excludeId = null);
    }
} 