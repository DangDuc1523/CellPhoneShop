using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Business.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }
    }
} 