using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Repositories.Interfaces;

public interface IPhoneAttributeRepository
{
    Task<IEnumerable<PhoneAttribute>> GetAllAsync();
    Task<PhoneAttribute?> GetByIdAsync(int id);
    Task<PhoneAttribute> CreateAsync(PhoneAttribute phoneAttribute);
    Task<PhoneAttribute> UpdateAsync(PhoneAttribute phoneAttribute);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
} 