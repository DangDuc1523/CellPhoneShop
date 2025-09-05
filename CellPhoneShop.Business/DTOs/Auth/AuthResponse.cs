using CellPhoneShop.Domain.Enums;

namespace CellPhoneShop.Business.DTOs.Auth
{
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Role Role { get; set; }
        public string Token { get; set; } = null!;
    }
} 