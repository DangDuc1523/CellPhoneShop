using CellPhoneShop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Repositories.Interfaces
{
    public interface IVariantAttributeRepository
    {
        Task<IEnumerable<VariantAttribute>> GetAllAsync();
        Task<VariantAttribute?> GetByIdAsync(int id);
        Task<VariantAttribute> CreateAsync(VariantAttribute variantAttribute);
        Task<VariantAttribute> UpdateAsync(VariantAttribute variantAttribute);
        Task<bool> DeleteAsync(int id);
    }
} 