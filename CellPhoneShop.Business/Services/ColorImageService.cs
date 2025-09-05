using AutoMapper;
using CellPhoneShop.Business.DTOs;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Services
{
    public class ColorImageService : IColorImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColorImageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColorImageDto>> GetByColorIdAsync(int colorId)
        {
            var colorImages = await _unitOfWork.ColorImages.GetByColorIdAsync(colorId);
            return _mapper.Map<IEnumerable<ColorImageDto>>(colorImages);
        }

        public async Task<ColorImageDto> GetByIdAsync(int id)
        {
            var colorImage = await _unitOfWork.ColorImages.GetByIdAsync(id);
            if (colorImage == null)
                throw new KeyNotFoundException($"ColorImage with ID {id} not found");

            return _mapper.Map<ColorImageDto>(colorImage);
        }

        public async Task<ColorImageDto> CreateAsync(CreateColorImageDto dto)
        {
            var colorImage = _mapper.Map<ColorImage>(dto);
            var createdColorImage = await _unitOfWork.ColorImages.CreateAsync(colorImage);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ColorImageDto>(createdColorImage);
        }

        public async Task UpdateAsync(int id, UpdateColorImageDto dto)
        {
            if (!await _unitOfWork.ColorImages.ExistsAsync(id))
                throw new KeyNotFoundException($"ColorImage with ID {id} not found");

            var colorImage = _mapper.Map<ColorImage>(dto);
            colorImage.ImageId = id;

            await _unitOfWork.ColorImages.UpdateAsync(colorImage);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _unitOfWork.ColorImages.ExistsAsync(id))
                throw new KeyNotFoundException($"ColorImage with ID {id} not found");

            await _unitOfWork.ColorImages.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 