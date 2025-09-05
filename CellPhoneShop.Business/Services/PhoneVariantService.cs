using AutoMapper;
using CellPhoneShop.Business.DTOs;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Services
{
    public class PhoneVariantService : IPhoneVariantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PhoneVariantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PhoneVariantDto>> GetByPhoneIdAsync(int phoneId)
        {
            var variants = await _unitOfWork.PhoneVariants.GetByPhoneIdAsync(phoneId);
            return _mapper.Map<IEnumerable<PhoneVariantDto>>(variants);
        }

        public async Task<IEnumerable<PhoneVariantDto>> GetByColorIdAsync(int colorId)
        {
            var variants = await _unitOfWork.PhoneVariants.GetByColorIdAsync(colorId);
            return _mapper.Map<IEnumerable<PhoneVariantDto>>(variants);
        }

        public async Task<PhoneVariantDto> GetByIdAsync(int id)
        {
            var variant = await _unitOfWork.PhoneVariants.GetByIdAsync(id);
            if (variant == null)
                throw new KeyNotFoundException($"PhoneVariant with ID {id} not found");

            return _mapper.Map<PhoneVariantDto>(variant);
        }

        public async Task<PhoneVariantDto> CreateAsync(CreatePhoneVariantDto dto)
        {
            // Validate that Phone exists
            if (!await _unitOfWork.Phones.ExistsAsync(dto.PhoneId))
                throw new KeyNotFoundException($"Phone with ID {dto.PhoneId} not found");

            // Validate that Color exists
            if (!await _unitOfWork.Colors.ExistsAsync(dto.ColorId))
                throw new KeyNotFoundException($"Color with ID {dto.ColorId} not found");

            // Validate SKU uniqueness if provided
            if (!string.IsNullOrEmpty(dto.Sku) && !await _unitOfWork.PhoneVariants.IsSkuUniqueAsync(dto.Sku))
                throw new InvalidOperationException($"SKU {dto.Sku} is already in use");

            var variant = _mapper.Map<PhoneVariant>(dto);
            var createdVariant = await _unitOfWork.PhoneVariants.CreateAsync(variant);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PhoneVariantDto>(createdVariant);
        }

        public async Task UpdateAsync(int id, UpdatePhoneVariantDto dto)
        {
            var existingVariant = await _unitOfWork.PhoneVariants.GetByIdAsync(id);
            if (existingVariant == null)
                throw new KeyNotFoundException($"PhoneVariant with ID {id} not found");

            // Validate SKU uniqueness if provided and changed
            if (!string.IsNullOrEmpty(dto.Sku) && 
                dto.Sku != existingVariant.Sku && 
                !await _unitOfWork.PhoneVariants.IsSkuUniqueAsync(dto.Sku, id))
                throw new InvalidOperationException($"SKU {dto.Sku} is already in use");

            var variant = _mapper.Map<PhoneVariant>(dto);
            variant.VariantId = id;
            variant.PhoneId = existingVariant.PhoneId;
            variant.ColorId = existingVariant.ColorId;

            await _unitOfWork.PhoneVariants.UpdateAsync(variant);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _unitOfWork.PhoneVariants.ExistsAsync(id))
                throw new KeyNotFoundException($"PhoneVariant with ID {id} not found");

            await _unitOfWork.PhoneVariants.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 