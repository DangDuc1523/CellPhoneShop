using CellPhoneShop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Repositories.Interfaces
{
    public interface IVariantAttributeValueRepository
    {
        Task<IEnumerable<VariantAttributeValue>> GetAllAsync();
        Task<IEnumerable<VariantAttributeValue>> GetByAttributeIdAsync(int attributeId);
        Task<VariantAttributeValue?> GetByIdAsync(int id);
        Task<VariantAttributeValue> CreateAsync(VariantAttributeValue value);
        Task<VariantAttributeValue> UpdateAsync(VariantAttributeValue value);
        Task<bool> DeleteAsync(int id);
    }
} 