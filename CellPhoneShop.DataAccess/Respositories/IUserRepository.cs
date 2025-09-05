using CellPhoneShop.Domain.Models;

namespace CellPhoneShop.DataAccess.Respositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserAccount>> GetAll();

        Task<UserAccount?> GetByIdAsync(int id);
        Task<UserAccount?> GetByEmailAsync(string email);
        Task<bool> IsEmailExistsAsync(string email);
        Task<UserAccount> CreateAsync(UserAccount user);
        Task UpdateUserAsync(UserAccount user);
        Task DeleteAsync(int id);
        Task<bool> changePassword(int id, string newPassword);
        Task<bool> UpdatePasswordByEmailAsync(string email, string newPassword);
    }
}