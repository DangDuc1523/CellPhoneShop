using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Business.DTOs.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
} 