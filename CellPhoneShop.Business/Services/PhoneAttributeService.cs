using CellPhoneShop.Business.DTOs.Attribute;
using CellPhoneShop.Business.Services.Interfaces;
using CellPhoneShop.DataAccess.Repositories.Interfaces;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Services;

public class PhoneAttributeService : IPhoneAttributeService
{
    private readonly IPhoneAttributeRepository _phoneAttributeRepository;

    public PhoneAttributeService(IPhoneAttributeRepository phoneAttributeRepository)
    {
        _phoneAttributeRepository = phoneAttributeRepository;
    }

    public async Task<IEnumerable<PhoneAttributeDto>> GetAllAsync()
    {
        var attributes = await _phoneAttributeRepository.GetAllAsync();
        return attributes.Select(MapToDto);
    }

    public async Task<PhoneAttributeDto?> GetByIdAsync(int id)
    {
        var attribute = await _phoneAttributeRepository.GetByIdAsync(id);
        return attribute != null ? MapToDto(attribute) : null;
    }

    public async Task<PhoneAttributeDto> CreateAsync(CreatePhoneAttributeDto createDto)
    {
        var phoneAttribute = new PhoneAttribute
        {
            Name = createDto.Name,
            Description = createDto.Description
        };

        var created = await _phoneAttributeRepository.CreateAsync(phoneAttribute);
        return MapToDto(created);
    }

    public async Task<PhoneAttributeDto> UpdateAsync(int id, UpdatePhoneAttributeDto updateDto)
    {
        var existing = await _phoneAttributeRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("Phone attribute not found", nameof(id));

        existing.Name = updateDto.Name;
        existing.Description = updateDto.Description;

        var updated = await _phoneAttributeRepository.UpdateAsync(existing);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _phoneAttributeRepository.DeleteAsync(id);
    }

    private static PhoneAttributeDto MapToDto(PhoneAttribute phoneAttribute)
    {
        return new PhoneAttributeDto
        {
            AttributeId = phoneAttribute.AttributeId,
            Name = phoneAttribute.Name,
            Description = phoneAttribute.Description,
            CreatedAt = phoneAttribute.CreatedAt,
            CreatedBy = phoneAttribute.CreatedBy,
            ModifiedAt = phoneAttribute.ModifiedAt,
            ModifiedBy = phoneAttribute.ModifiedBy,
            IsDeleted = phoneAttribute.IsDeleted
        };
    }
} 