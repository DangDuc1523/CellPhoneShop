using CellPhoneShop.Business.DTOs;

namespace CellPhoneShop.Business.Services
{
    public interface IPhoneVariantService
    {
        Task<IEnumerable<PhoneVariantDto>> GetByPhoneIdAsync(int phoneId);
        Task<IEnumerable<PhoneVariantDto>> GetByColorIdAsync(int colorId);
        Task<PhoneVariantDto> GetByIdAsync(int id);
        Task<PhoneVariantDto> CreateAsync(CreatePhoneVariantDto dto);
        Task UpdateAsync(int id, UpdatePhoneVariantDto dto);
        Task DeleteAsync(int id);
    }
} 