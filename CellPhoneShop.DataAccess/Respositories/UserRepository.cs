using CellPhoneShop.DataAccess.Respositories.Interfaces;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CellPhoneShop.DataAccess.Respositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CellPhoneShopContext _context;

        public UserRepository(CellPhoneShopContext context)
        {
            _context = context;
        }

        public async Task<UserAccount?> GetByIdAsync(int id)
        {
            return await _context.UserAccounts
                .FirstOrDefaultAsync(u => u.UserId == id && u.IsDeleted != true);
        }

        public async Task<UserAccount?> GetByEmailAsync(string email)
        {
            return await _context.UserAccounts
                .FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted != true);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.UserAccounts
                .AnyAsync(u => u.Email == email && u.IsDeleted != true);
        }

        public async Task<UserAccount> CreateAsync(UserAccount user)
        {
            _context.UserAccounts.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(UserAccount user)
        {
            user.ModifiedAt = DateTime.Now;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserAccount>> GetAll()
        {
            return await _context.UserAccounts.Where(p => p.IsDeleted != true).ToListAsync();
        }

        public async Task<bool> changePassword(int id, string newPassword)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                user.PasswordHash = newPassword;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        //DANGDUC
        public async Task<bool> UpdatePasswordByEmailAsync(string email, string newPassword)
        {
           var user = await GetByEmailAsync(email);
            if (user != null) 
            {
                user.PasswordHash = newPassword;
                await _context.SaveChangesAsync();
            }
            else
            {
                return false;
            }
            return true;
            
        }
        //DANGDUC

    }
}