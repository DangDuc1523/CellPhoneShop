using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Business.DTOs
{
    public class ColorDto
    {
        public int ColorId { get; set; }
        public int? PhoneId { get; set; }
        public string ColorName { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CreateColorDto
    {
        [Required(ErrorMessage = "Phone ID is required")]
        public int PhoneId { get; set; }

        [Required(ErrorMessage = "Color name is required")]
        [StringLength(50, ErrorMessage = "Color name cannot exceed 50 characters")]
        public string ColorName { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        public string ImageUrl { get; set; }
    }

    public class UpdateColorDto
    {
        [Required(ErrorMessage = "Color name is required")]
        [StringLength(50, ErrorMessage = "Color name cannot exceed 50 characters")]
        public string ColorName { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        public string ImageUrl { get; set; }
    }
} 