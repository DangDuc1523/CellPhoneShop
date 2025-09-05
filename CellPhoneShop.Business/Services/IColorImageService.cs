using CellPhoneShop.Business.DTOs;

namespace CellPhoneShop.Business.Services
{
    public interface IColorImageService
    {
        Task<IEnumerable<ColorImageDto>> GetByColorIdAsync(int colorId);
        Task<ColorImageDto> GetByIdAsync(int id);
        Task<ColorImageDto> CreateAsync(CreateColorImageDto dto);
        Task UpdateAsync(int id, UpdateColorImageDto dto);
        Task DeleteAsync(int id);
    }
} 