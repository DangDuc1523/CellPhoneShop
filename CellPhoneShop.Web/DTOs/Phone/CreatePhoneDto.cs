using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Web.DTOs.Phone;

public class CreatePhoneDto
{
    [Required(ErrorMessage = "Brand is required")]
    [Display(Name = "Brand")]
    public int? BrandId { get; set; }

    [Required(ErrorMessage = "Phone name is required")]
    [StringLength(200, ErrorMessage = "Phone name cannot exceed 200 characters")]
    [Display(Name = "Phone Name")]
    public string PhoneName { get; set; } = null!;

    [Required(ErrorMessage = "Base price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Base price must be greater than 0")]
    [Display(Name = "Base Price")]
    public decimal BasePrice { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [Display(Name = "Phone Attributes")]
    public List<PhoneAttributeMappingDto> AttributeMappings { get; set; } = new();

    public List<int>? SelectedAttributeIds { get; set; }
}
