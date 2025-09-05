using CellPhoneShop.Business.DTOs.Attribute;

namespace CellPhoneShop.Business.Services.Interfaces;

public interface IPhoneAttributeService
{
    Task<IEnumerable<PhoneAttributeDto>> GetAllAsync();
    Task<PhoneAttributeDto?> GetByIdAsync(int id);
    Task<PhoneAttributeDto> CreateAsync(CreatePhoneAttributeDto createDto);
    Task<PhoneAttributeDto> UpdateAsync(int id, UpdatePhoneAttributeDto updateDto);
    Task<bool> DeleteAsync(int id);
} 