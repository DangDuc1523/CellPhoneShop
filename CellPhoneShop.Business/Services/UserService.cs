using AutoMapper;
using CellPhoneShop.Business.DTOs;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllAccounts()
        {
            var users = await _unitOfWork.Users.GetAll();
            return _mapper.Map<IEnumerable<UserProfileDto>>(users);
        }

        public async Task<UserAccount?> GetByIdAsync(int id)
        {
            UserAccount users = await _unitOfWork.Users.GetByIdAsync(id);
            return users;
        }

        public async Task<UserProfileDto?> GetByEmailAsync(string email)
        {
            var users = await _unitOfWork.Users.GetByEmailAsync(email);
            return _mapper.Map<UserProfileDto>(users);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var users = await _unitOfWork.Users.IsEmailExistsAsync(email);
            return users;
        }

        public async Task UpdateUserAsync(UserAccount user)
        {
            await _unitOfWork.Users.UpdateUserAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Users.DeleteAsync(id);
        }

        public async Task<UserAccount> CreateAsync(UserAccount user)
        {
            return await _unitOfWork.Users.CreateAsync(user);
        }

        public async Task<bool> ChangePassWord(int id, string newPassword)
        {
            return await _unitOfWork.Users.changePassword(id, newPassword);
        }

        public async Task<bool> UpdatePasswordByEmailAsync(string Email, string NewPassword)
        {
            return await _unitOfWork.Users.UpdatePasswordByEmailAsync(Email, NewPassword);
        }
    }
}
