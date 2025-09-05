using AutoMapper;
using CellPhoneShop.Business.DTOs;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Services
{
    public class ColorService : IColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColorDto>> GetByPhoneIdAsync(int phoneId)
        {
            var colors = await _unitOfWork.Colors.GetByPhoneIdAsync(phoneId);
            return _mapper.Map<IEnumerable<ColorDto>>(colors);
        }

        public async Task<ColorDto> GetByIdAsync(int id)
        {
            var color = await _unitOfWork.Colors.GetByIdAsync(id);
            if (color == null)
                throw new KeyNotFoundException($"Color with ID {id} not found");

            return _mapper.Map<ColorDto>(color);
        }

        public async Task<ColorDto> CreateAsync(CreateColorDto createDto, int userId)
        {
            var color = _mapper.Map<Color>(createDto);
            color.CreatedBy = userId;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var createdColor = await _unitOfWork.Colors.CreateAsync(color);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ColorDto>(createdColor);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<ColorDto> UpdateAsync(int id, UpdateColorDto updateDto, int userId)
        {
            var existingColor = await _unitOfWork.Colors.GetByIdAsync(id);
            if (existingColor == null)
                throw new KeyNotFoundException($"Color with ID {id} not found");

            _mapper.Map(updateDto, existingColor);
            existingColor.ModifiedBy = userId;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Colors.UpdateAsync(existingColor);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ColorDto>(existingColor);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var color = await _unitOfWork.Colors.GetByIdAsync(id);
            if (color == null)
                throw new KeyNotFoundException($"Color with ID {id} not found");

            color.DeletedBy = userId;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Colors.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
} 