using CellPhoneShop.Business.DTOs;

namespace CellPhoneShop.Business.Services
{
    public interface IPhoneService
    {
        Task<IEnumerable<PhoneDto>> GetAllAsync();
        Task<PhoneDto> GetByIdAsync(int id);
        Task<PhoneSearchResultDto> SearchAsync(PhoneSearchDto searchDto);
        Task<PhoneDto> CreateAsync(CreatePhoneDto createDto, int userId);
        Task<PhoneDto> UpdateAsync(int id, UpdatePhoneDto updateDto, int userId);
        Task DeleteAsync(int id, int userId);
        Task<IEnumerable<PhoneDto>> SearchPhonesAsync(string query);
    }
} 