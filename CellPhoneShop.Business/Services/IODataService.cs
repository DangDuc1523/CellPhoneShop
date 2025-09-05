using CellPhoneShop.Business.DTOs.OData;

namespace CellPhoneShop.Business.Services
{
    public interface IODataService
    {
        IQueryable<PhoneVariantODataDto> GetPhoneVariantsQueryable();
        Task<IEnumerable<PhoneVariantODataDto>> GetPhoneVariantsAsync();
        Task<PhoneVariantODataDto?> GetPhoneVariantByIdAsync(int variantId);
    }
} 