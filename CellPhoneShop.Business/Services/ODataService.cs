using CellPhoneShop.Business.DTOs.OData;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CellPhoneShop.Business.Services
{
    public class ODataService : IODataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CellPhoneShopContext _context;

        public ODataService(IUnitOfWork unitOfWork, CellPhoneShopContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IQueryable<PhoneVariantODataDto> GetPhoneVariantsQueryable()
        {
            return _context.PhoneVariants
                .Where(pv => pv.IsDeleted != true && pv.Status == 1) // Only active variants
                .Include(pv => pv.Phone)
                    .ThenInclude(p => p.Brand)
                .Include(pv => pv.Phone)
                    .ThenInclude(p => p.PhoneAttributeMappings)
                        .ThenInclude(pam => pam.Attribute)
                .Include(pv => pv.Color)
                .Select(pv => new PhoneVariantODataDto
                {
                    VariantId = pv.VariantId,
                    PhoneId = pv.PhoneId ?? 0,
                    ColorId = pv.ColorId,
                    Price = pv.Price,
                    Stock = pv.Stock,
                    Sku = pv.Sku,
                    Status = pv.Status,
                    IsDefault = pv.IsDefault ?? false,
                    CreatedAt = pv.CreatedAt,
                    
                    // Phone information
                    PhoneName = pv.Phone!.PhoneName,
                    PhoneDescription = pv.Phone.Description,
                    PhoneBasePrice = pv.Phone.BasePrice,
                    BrandName = pv.Phone.Brand!.BrandName,
                    BrandId = pv.Phone.BrandId ?? 0,
                    
                    // Color information
                    ColorName = pv.Color != null ? pv.Color.ColorName : null,
                    ColorImageUrl = pv.Color != null ? pv.Color.ImageUrl : null,
                    
                    // Navigation properties
                    Phone = new PhoneODataDto
                    {
                        PhoneId = pv.Phone.PhoneId,
                        PhoneName = pv.Phone.PhoneName,
                        Description = pv.Phone.Description,
                        BasePrice = pv.Phone.BasePrice,
                        BrandId = pv.Phone.BrandId ?? 0,
                        BrandName = pv.Phone.Brand!.BrandName,
                        CreatedAt = pv.Phone.CreatedAt,
                        PhoneAttributes = pv.Phone.PhoneAttributeMappings
                            .Where(pam => pam.IsDeleted != true)
                            .Select(pam => new PhoneAttributeODataDto
                            {
                                MappingId = pam.PhoneId * 1000 + pam.AttributeId,
                                PhoneId = pam.PhoneId,
                                AttributeId = pam.AttributeId,
                                AttributeName = pam.Attribute!.Name,
                                Value = pam.Value
                            }).ToList()
                    },
                    
                    Color = pv.Color != null ? new ColorODataDto
                    {
                        ColorId = pv.Color.ColorId,
                        ColorName = pv.Color.ColorName,
                        ImageUrl = pv.Color.ImageUrl
                    } : null,
                    
                    // TODO: Add VariantAttributes when VariantAttributeMapping is implemented
                    VariantAttributes = new List<VariantAttributeODataDto>()
                });
        }

        public async Task<IEnumerable<PhoneVariantODataDto>> GetPhoneVariantsAsync()
        {
            return await GetPhoneVariantsQueryable().ToListAsync();
        }

        public async Task<PhoneVariantODataDto?> GetPhoneVariantByIdAsync(int variantId)
        {
            return await GetPhoneVariantsQueryable()
                .FirstOrDefaultAsync(pv => pv.VariantId == variantId);
        }
    }
} 