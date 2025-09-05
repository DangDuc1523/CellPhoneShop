using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Respositories
{
    public interface IPhoneVariantRepository
    {
        Task<IEnumerable<PhoneVariant>> GetByPhoneIdAsync(int phoneId);
        Task<IEnumerable<PhoneVariant>> GetByColorIdAsync(int colorId);
        Task<PhoneVariant> GetByIdAsync(int id);
        Task<PhoneVariant> CreateAsync(PhoneVariant phoneVariant);
        Task UpdateAsync(PhoneVariant phoneVariant);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsSkuUniqueAsync(string sku, int? excludeVariantId = null);
    }
} 