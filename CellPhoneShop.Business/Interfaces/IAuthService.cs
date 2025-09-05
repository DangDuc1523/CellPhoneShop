using CellPhoneShop.Business.DTOs.Auth;

namespace CellPhoneShop.Business.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<bool> IsEmailExistsAsync(string email);
    }
} 