using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Web.DTOs.Attribute
{
    public class CreateVariantAttributeDto
    {
        [Required(ErrorMessage = "Attribute name is required")]
        [StringLength(100, ErrorMessage = "Attribute name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public List<string> Values { get; set; } = new();
    }
} 