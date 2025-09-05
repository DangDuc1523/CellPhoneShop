using CellPhoneShop.API.DTOs.Brand;

namespace CellPhoneShop.Business.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
        Task<BrandDto> GetBrandByIdAsync(int id);
        Task<BrandDto> CreateBrandAsync(CreateBrandDto createDto, int userId);
        Task<BrandDto> UpdateBrandAsync(int id, UpdateBrandDto updateDto, int userId);
        Task DeleteBrandAsync(int id, int userId);
        Task<bool> BrandExistsAsync(int id);
    }
} 