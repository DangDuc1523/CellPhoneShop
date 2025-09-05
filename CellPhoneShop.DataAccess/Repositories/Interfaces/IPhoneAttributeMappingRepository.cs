using CellPhoneShop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CellPhoneShop.DataAccess.Repositories.Interfaces
{
    public interface IPhoneAttributeMappingRepository
    {
        Task<List<PhoneAttributeMapping>> GetByPhoneIdAsync(int phoneId);
        Task AddRangeAsync(IEnumerable<PhoneAttributeMapping> mappings);
        Task DeleteByPhoneIdAsync(int phoneId);
    }
} 