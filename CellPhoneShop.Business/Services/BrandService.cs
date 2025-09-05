using AutoMapper;
using CellPhoneShop.API.DTOs.Brand;
using CellPhoneShop.Business.Interfaces;
using CellPhoneShop.DataAccess.Respositories.Interfaces;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Brands.GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDto>>(brands);
        }

        public async Task<BrandDto> GetBrandByIdAsync(int id)
        {
            var brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {id} not found");

            return _mapper.Map<BrandDto>(brand);
        }

        public async Task<BrandDto> CreateBrandAsync(CreateBrandDto createDto, int userId)
        {
            if (!await _unitOfWork.Brands.IsBrandNameUniqueAsync(createDto.BrandName))
                throw new InvalidOperationException($"Brand name '{createDto.BrandName}' is already in use");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException($"User with ID {userId} does not exist.");

            var brand = _mapper.Map<Brand>(createDto);
            brand.CreatedBy = userId;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var createdBrand = await _unitOfWork.Brands.CreateAsync(brand);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<BrandDto>(createdBrand);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<BrandDto> UpdateBrandAsync(int id, UpdateBrandDto updateDto, int userId)
        {
            if (!await _unitOfWork.Brands.IsBrandNameUniqueAsync(updateDto.BrandName, id))
                throw new InvalidOperationException($"Brand name '{updateDto.BrandName}' is already in use");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException($"User with ID {userId} does not exist.");

            var brand = _mapper.Map<Brand>(updateDto);
            brand.BrandId = id;
            brand.ModifiedBy = userId;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Brands.UpdateAsync(brand);
                await _unitOfWork.CommitAsync();

                var updatedBrand = await _unitOfWork.Brands.GetByIdAsync(id);
                return _mapper.Map<BrandDto>(updatedBrand);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteBrandAsync(int id, int userId)
        {
            if (!await _unitOfWork.Brands.ExistsAsync(id))
                throw new KeyNotFoundException($"Brand with ID {id} not found");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException($"User with ID {userId} does not exist.");

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Brands.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> BrandExistsAsync(int id)
        {
            return await _unitOfWork.Brands.ExistsAsync(id);
        }
    }
} 