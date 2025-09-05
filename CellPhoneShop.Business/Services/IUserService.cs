using CellPhoneShop.Business.DTOs;
using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Services
{
    public interface IUserService
    {

        Task<IEnumerable<UserProfileDto>> GetAllAccounts();
        Task<UserAccount> GetByIdAsync(int id);
        Task<UserProfileDto> GetByEmailAsync(string email);
        Task<bool> IsEmailExistsAsync(string email);
        Task<UserAccount> CreateAsync(UserAccount user);
        Task UpdateUserAsync(UserAccount user);
        Task DeleteAsync(int id);
        Task<bool> ChangePassWord(int id, string passWord);
        Task<bool> UpdatePasswordByEmailAsync(string Email, string NewPassword);
    }
}
