using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Business.DTOs
{
    public class ColorImageDto
    {
        public int ImageId { get; set; }
        public int? ColorId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CreateColorImageDto
    {
        [Required(ErrorMessage = "Color ID is required")]
        public int ColorId { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        public string ImageUrl { get; set; }
    }

    public class UpdateColorImageDto
    {
        [Required(ErrorMessage = "Image URL is required")]
        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        public string ImageUrl { get; set; }
    }
} 