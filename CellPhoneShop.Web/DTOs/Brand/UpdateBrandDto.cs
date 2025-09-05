using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Web.DTOs.Brand
{
    public class UpdateBrandDto
    {
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Brand name is required")]
        [StringLength(100, ErrorMessage = "Brand name cannot exceed 100 characters")]
        public string BrandName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
} 