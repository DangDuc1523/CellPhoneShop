using CellPhoneShop.Business.DTOs.Attribute;

namespace CellPhoneShop.Business.Services.Interfaces;

public interface IVariantAttributeService
{
    Task<IEnumerable<VariantAttributeDto>> GetAllAsync();
    Task<VariantAttributeDto?> GetByIdAsync(int id);
    Task<VariantAttributeDto> CreateAsync(CreateVariantAttributeDto createDto);
    Task<VariantAttributeDto> UpdateAsync(int id, UpdateVariantAttributeDto updateDto);
    Task<bool> DeleteAsync(int id);
}

public interface IVariantAttributeValueService
{
    Task<IEnumerable<VariantAttributeValueDto>> GetAllAsync();
    Task<IEnumerable<VariantAttributeValueDto>> GetByAttributeIdAsync(int attributeId);
    Task<VariantAttributeValueDto?> GetByIdAsync(int id);
    Task<VariantAttributeValueDto> CreateAsync(CreateVariantAttributeValueDto createDto);
    Task<VariantAttributeValueDto> UpdateAsync(int id, UpdateVariantAttributeValueDto updateDto);
    Task<bool> DeleteAsync(int id);
} 