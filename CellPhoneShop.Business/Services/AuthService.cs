using CellPhoneShop.Business.DTOs.Auth;
using CellPhoneShop.Business.Interfaces;
using CellPhoneShop.DataAccess.Respositories.Interfaces;
using CellPhoneShop.DataAccess.Respositories;
using CellPhoneShop.Domain.Enums;
using CellPhoneShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace CellPhoneShop.Business.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IConfiguration _configuration;

		public AuthService(IUserRepository userRepository, IConfiguration configuration)
		{
			_userRepository = userRepository;
			_configuration = configuration;
		}

		public async Task<AuthResponse> LoginAsync(LoginRequest request)
		{
			var user = await _userRepository.GetByEmailAsync(request.Email);
			if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
				throw new UnauthorizedAccessException("Invalid email or password");

			if (user.IsDeleted == true)
				throw new UnauthorizedAccessException("This account has been deleted");

			return new AuthResponse
			{
				UserId = user.UserId,
				FullName = user.FullName ?? "",
				Email = user.Email,
				Role = (Role)user.Role,
				Token = GenerateJwtToken(user)
			};
		}

		public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
		{
			if (await IsEmailExistsAsync(request.Email))
				throw new InvalidOperationException("Email already exists");

			var user = new UserAccount
			{
				FullName = request.FullName,
				Email = request.Email,
				PasswordHash = HashPassword(request.Password),
				Phone = request.Phone,
				Role = (int)Role.Customer,
				Status = 1, // Active
				CreatedAt = DateTime.Now
			};

			await _userRepository.CreateAsync(user);

			return new AuthResponse
			{
				UserId = user.UserId,
				FullName = user.FullName ?? "",
				Email = user.Email,
				Role = (Role)user.Role,
				Token = GenerateJwtToken(user)
			};
		}

		public async Task<bool> IsEmailExistsAsync(string email)
		{
			return await _userRepository.IsEmailExistsAsync(email);
		}

		private string GenerateJwtToken(UserAccount user)
		{
			var jwtConfig = _configuration.GetSection("Jwt");
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
				new Claim("userId", user.UserId.ToString()),
				new Claim("email", user.Email),
				new Claim(System.Security.Claims.ClaimTypes.Role, ((Role)user.Role).ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var token = new JwtSecurityToken(
				issuer: jwtConfig["Issuer"],
				audience: jwtConfig["Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtConfig["ExpireMinutes"])),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private string HashPassword(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				return Convert.ToBase64String(hashedBytes);
			}
		}

		private bool VerifyPassword(string password, string hash)
		{
			return HashPassword(password) == hash;
		}
	}
}



