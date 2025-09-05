using CellPhoneShop.Business.DTOs;

namespace CellPhoneShop.Business.Services
{
    public interface IColorService
    {
        Task<IEnumerable<ColorDto>> GetByPhoneIdAsync(int phoneId);
        Task<ColorDto> GetByIdAsync(int id);
        Task<ColorDto> CreateAsync(CreateColorDto createDto, int userId);
        Task<ColorDto> UpdateAsync(int id, UpdateColorDto updateDto, int userId);
        Task DeleteAsync(int id, int userId);
    }
} 