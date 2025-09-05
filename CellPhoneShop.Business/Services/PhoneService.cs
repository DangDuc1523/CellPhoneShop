using AutoMapper;
using CellPhoneShop.Business.DTOs;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.Business.Services
{
    public class PhoneService : IPhoneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PhoneService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PhoneDto>> GetAllAsync()
        {
            var phones = await _unitOfWork.Phones.GetAllAsync();
            return _mapper.Map<IEnumerable<PhoneDto>>(phones);
        }

        public async Task<PhoneDto> GetByIdAsync(int id)
        {
            var phone = await _unitOfWork.Phones.GetByIdAsync(id);
            if (phone == null)
                throw new KeyNotFoundException($"Phone with ID {id} not found");

            var phoneDto = _mapper.Map<PhoneDto>(phone);
            phoneDto.AttributeMappings = phone.PhoneAttributeMappings
                .Where(x => x.IsDeleted == false || x.IsDeleted == null)
                .Select(x => new PhoneAttributeMappingDto
                {
                    AttributeId = x.AttributeId,
                    Value = x.Value,
                    AttributeName = x.Attribute?.Name
                }).ToList();
            return phoneDto;
        }

        public async Task<PhoneSearchResultDto> SearchAsync(PhoneSearchDto searchDto)
        {
            var phones = await _unitOfWork.Phones.SearchAsync(
                searchDto.SearchTerm,
                searchDto.BrandId,
                searchDto.MinPrice,
                searchDto.MaxPrice,
                searchDto.Ram,
                searchDto.Os,
                searchDto.Page,
                searchDto.PageSize,
                searchDto.SortBy,
                searchDto.IsAscending);

            var totalCount = await _unitOfWork.Phones.GetTotalCountAsync(
                searchDto.SearchTerm,
                searchDto.BrandId,
                searchDto.MinPrice,
                searchDto.MaxPrice,
                searchDto.Ram,
                searchDto.Os);

            var totalPages = (int)Math.Ceiling(totalCount / (double)searchDto.PageSize);

            return new PhoneSearchResultDto
            {
                Items = _mapper.Map<IEnumerable<PhoneDto>>(phones),
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = searchDto.Page,
                PageSize = searchDto.PageSize
            };
        }

        public async Task<PhoneDto> CreateAsync(CreatePhoneDto createDto, int userId)
        {
            var phone = _mapper.Map<Phone>(createDto);
            phone.CreatedBy = userId;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var createdPhone = await _unitOfWork.Phones.CreateAsync(phone);
                if (createDto.AttributeMappings != null && createDto.AttributeMappings.Count > 0)
                {
                    var mappings = createDto.AttributeMappings.Select(x => new PhoneAttributeMapping
                    {
                        PhoneId = createdPhone.PhoneId,
                        AttributeId = x.AttributeId,
                        Value = string.IsNullOrWhiteSpace(x.Value) ? "N/A" : x.Value,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId,
                        IsDeleted = false
                    }).ToList();
                    await _unitOfWork.PhoneAttributeMappings.AddRangeAsync(mappings);
                }
                await _unitOfWork.CommitAsync();
                return _mapper.Map<PhoneDto>(createdPhone);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<PhoneDto> UpdateAsync(int id, UpdatePhoneDto updateDto, int userId)
        {
            var existingPhone = await _unitOfWork.Phones.GetByIdAsync(id);
            if (existingPhone == null)
                throw new KeyNotFoundException($"Phone with ID {id} not found");

            _mapper.Map(updateDto, existingPhone);
            existingPhone.ModifiedBy = userId;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Phones.UpdateAsync(existingPhone);
                
                // Log before deleting AttributeMappings
                Console.WriteLine($"Deleting AttributeMappings for Phone ID: {id}");
                await _unitOfWork.PhoneAttributeMappings.DeleteByPhoneIdAsync(id);
                
                if (updateDto.AttributeMappings != null && updateDto.AttributeMappings.Count > 0)
                {
                    var mappings = updateDto.AttributeMappings.Select(x => new PhoneAttributeMapping
                    {
                        PhoneId = id,
                        AttributeId = x.AttributeId,
                        Value = string.IsNullOrWhiteSpace(x.Value) ? "N/A" : x.Value,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId,
                        IsDeleted = false
                    }).ToList();
                    
                    // Log before adding new AttributeMappings
                    Console.WriteLine($"Adding {mappings.Count} new AttributeMappings for Phone ID: {id}");
                    foreach (var mapping in mappings)
                    {
                        Console.WriteLine($"  - AttributeId: {mapping.AttributeId}, Value: {mapping.Value}");
                    }
                    
                    await _unitOfWork.PhoneAttributeMappings.AddRangeAsync(mappings);
                }
                
                Console.WriteLine($"Committing transaction for Phone ID: {id}");
                await _unitOfWork.CommitAsync();
                return _mapper.Map<PhoneDto>(existingPhone);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdateAsync for Phone ID: {id}");
                Console.WriteLine($"Exception: {ex}");
                
                // Log inner exceptions
                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    Console.WriteLine($"Inner Exception: {innerEx}");
                    innerEx = innerEx.InnerException;
                }
                
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var phone = await _unitOfWork.Phones.GetByIdAsync(id);
            if (phone == null)
                throw new KeyNotFoundException($"Phone with ID {id} not found");

            phone.DeletedBy = userId;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Phones.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<PhoneDto>> SearchPhonesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<PhoneDto>();
            var phones = await _unitOfWork.Phones.GetAllAsync();
            var matched = phones
                .Where(p => p.PhoneName != null && p.PhoneName.ToLower().Contains(query.ToLower()))
                .Take(10)
                .ToList();
            return _mapper.Map<IEnumerable<PhoneDto>>(matched);
        }
    }
} 