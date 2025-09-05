using CellPhoneShop.Business.DTOs.Attribute;
using CellPhoneShop.Business.Services.Interfaces;
using CellPhoneShop.DataAccess.Repositories.Interfaces;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Services;

public class VariantAttributeService : IVariantAttributeService
{
    private readonly IVariantAttributeRepository _variantAttributeRepository;
    private readonly IVariantAttributeValueRepository _valueRepository;

    public VariantAttributeService(
        IVariantAttributeRepository variantAttributeRepository,
        IVariantAttributeValueRepository valueRepository)
    {
        _variantAttributeRepository = variantAttributeRepository;
        _valueRepository = valueRepository;
    }

    public async Task<IEnumerable<VariantAttributeDto>> GetAllAsync()
    {
        var attributes = await _variantAttributeRepository.GetAllAsync();
        return attributes.Select(MapToDto);
    }

    public async Task<VariantAttributeDto?> GetByIdAsync(int id)
    {
        var attribute = await _variantAttributeRepository.GetByIdAsync(id);
        return attribute != null ? MapToDto(attribute) : null;
    }

    public async Task<VariantAttributeDto> CreateAsync(CreateVariantAttributeDto createDto)
    {
        var variantAttribute = new VariantAttribute
        {
            Name = createDto.Name,
            Description = createDto.Description
        };

        var created = await _variantAttributeRepository.CreateAsync(variantAttribute);

        // Create values if provided
        if (createDto.Values.Any())
        {
            foreach (var value in createDto.Values)
            {
                var attributeValue = new VariantAttributeValue
                {
                    VariantAttributeId = created.VariantAttributeId,
                    Value = value
                };
                await _valueRepository.CreateAsync(attributeValue);
            }
        }

        return await GetByIdAsync(created.VariantAttributeId) ?? MapToDto(created);
    }

    public async Task<VariantAttributeDto> UpdateAsync(int id, UpdateVariantAttributeDto updateDto)
    {
        var existing = await _variantAttributeRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("Variant attribute not found", nameof(id));

        existing.Name = updateDto.Name;
        existing.Description = updateDto.Description;

        var updated = await _variantAttributeRepository.UpdateAsync(existing);

        // Update values
        var existingValues = await _valueRepository.GetByAttributeIdAsync(id);
        var existingValueIds = existingValues.Select(v => v.ValueId).ToList();
        var updatedValueIds = updateDto.Values.Select(v => v.ValueId).ToList();

        // Remove values that are no longer in the list
        foreach (var valueId in existingValueIds.Except(updatedValueIds))
        {
            await _valueRepository.DeleteAsync(valueId);
        }

        // Update existing values and add new ones
        foreach (var valueDto in updateDto.Values)
        {
            if (valueDto.ValueId > 0)
            {
                // Update existing value
                var existingValue = existingValues.FirstOrDefault(v => v.ValueId == valueDto.ValueId);
                if (existingValue != null)
                {
                    existingValue.Value = valueDto.Value;
                    await _valueRepository.UpdateAsync(existingValue);
                }
            }
            else
            {
                // Create new value
                var newValue = new VariantAttributeValue
                {
                    VariantAttributeId = id,
                    Value = valueDto.Value
                };
                await _valueRepository.CreateAsync(newValue);
            }
        }

        return await GetByIdAsync(id) ?? MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _variantAttributeRepository.DeleteAsync(id);
    }

    private static VariantAttributeDto MapToDto(VariantAttribute variantAttribute)
    {
        return new VariantAttributeDto
        {
            VariantAttributeId = variantAttribute.VariantAttributeId,
            Name = variantAttribute.Name,
            Description = variantAttribute.Description,
            CreatedAt = variantAttribute.CreatedAt,
            CreatedBy = variantAttribute.CreatedBy,
            ModifiedAt = variantAttribute.ModifiedAt,
            ModifiedBy = variantAttribute.ModifiedBy,
            IsDeleted = variantAttribute.IsDeleted,
            Values = variantAttribute.VariantAttributeValues.Select(MapValueToDto).ToList()
        };
    }

    private static VariantAttributeValueDto MapValueToDto(VariantAttributeValue value)
    {
        return new VariantAttributeValueDto
        {
            ValueId = value.ValueId,
            VariantAttributeId = value.VariantAttributeId,
            Value = value.Value,
            CreatedAt = value.CreatedAt,
            CreatedBy = value.CreatedBy,
            ModifiedAt = value.ModifiedAt,
            ModifiedBy = value.ModifiedBy,
            IsDeleted = value.IsDeleted
        };
    }
}

public class VariantAttributeValueService : IVariantAttributeValueService
{
    private readonly IVariantAttributeValueRepository _valueRepository;

    public VariantAttributeValueService(IVariantAttributeValueRepository valueRepository)
    {
        _valueRepository = valueRepository;
    }

    public async Task<IEnumerable<VariantAttributeValueDto>> GetAllAsync()
    {
        var values = await _valueRepository.GetAllAsync();
        return values.Select(MapToDto);
    }

    public async Task<IEnumerable<VariantAttributeValueDto>> GetByAttributeIdAsync(int attributeId)
    {
        var values = await _valueRepository.GetByAttributeIdAsync(attributeId);
        return values.Select(MapToDto);
    }

    public async Task<VariantAttributeValueDto?> GetByIdAsync(int id)
    {
        var value = await _valueRepository.GetByIdAsync(id);
        return value != null ? MapToDto(value) : null;
    }

    public async Task<VariantAttributeValueDto> CreateAsync(CreateVariantAttributeValueDto createDto)
    {
        var value = new VariantAttributeValue
        {
            VariantAttributeId = createDto.VariantAttributeId,
            Value = createDto.Value
        };

        var created = await _valueRepository.CreateAsync(value);
        return MapToDto(created);
    }

    public async Task<VariantAttributeValueDto> UpdateAsync(int id, UpdateVariantAttributeValueDto updateDto)
    {
        var existing = await _valueRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("Variant attribute value not found", nameof(id));

        existing.Value = updateDto.Value;

        var updated = await _valueRepository.UpdateAsync(existing);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _valueRepository.DeleteAsync(id);
    }

    private static VariantAttributeValueDto MapToDto(VariantAttributeValue value)
    {
        return new VariantAttributeValueDto
        {
            ValueId = value.ValueId,
            VariantAttributeId = value.VariantAttributeId,
            Value = value.Value,
            CreatedAt = value.CreatedAt,
            CreatedBy = value.CreatedBy,
            ModifiedAt = value.ModifiedAt,
            ModifiedBy = value.ModifiedBy,
            IsDeleted = value.IsDeleted
        };
    }
} 