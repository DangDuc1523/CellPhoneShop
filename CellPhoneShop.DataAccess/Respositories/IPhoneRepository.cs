using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Respositories
{
    public interface IPhoneRepository
    {
        Task<IEnumerable<Phone>> GetAllAsync();
        Task<Phone> GetByIdAsync(int id);
        Task<IEnumerable<Phone>> SearchAsync(
            string? searchTerm,
            int? brandId,
            decimal? minPrice,
            decimal? maxPrice,
            string? ram,
            string? os,
            int page = 1,
            int pageSize = 10,
            string? sortBy = null,
            bool isAscending = true
        );
        Task<int> GetTotalCountAsync(
            string? searchTerm = null,
            int? brandId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? ram = null,
            string? os = null
        );
        Task<Phone> CreateAsync(Phone phone);
        Task UpdateAsync(Phone phone);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 