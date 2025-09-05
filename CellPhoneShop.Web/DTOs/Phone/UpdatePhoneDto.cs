using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CellPhoneShop.Web.DTOs.Phone
{
    public class UpdatePhoneDto
    {
        public int PhoneId { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public int? BrandId { get; set; }

        [Required(ErrorMessage = "Phone name is required")]
        [StringLength(200, ErrorMessage = "Phone name cannot exceed 200 characters")]
        public string PhoneName { get; set; }

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Base price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Base price must be greater than or equal to 0")]
        public decimal BasePrice { get; set; }

        [StringLength(100, ErrorMessage = "Screen specification cannot exceed 100 characters")]
        public string? Screen { get; set; }

        [StringLength(50, ErrorMessage = "OS cannot exceed 50 characters")]
        public string? Os { get; set; }

        [StringLength(100, ErrorMessage = "Front camera specification cannot exceed 100 characters")]
        public string? FrontCamera { get; set; }

        [StringLength(100, ErrorMessage = "Rear camera specification cannot exceed 100 characters")]
        public string? RearCamera { get; set; }

        [StringLength(100, ErrorMessage = "CPU specification cannot exceed 100 characters")]
        public string? Cpu { get; set; }

        [StringLength(50, ErrorMessage = "RAM specification cannot exceed 50 characters")]
        public string? Ram { get; set; }

        [StringLength(50, ErrorMessage = "Battery specification cannot exceed 50 characters")]
        public string? Battery { get; set; }

        [StringLength(50, ErrorMessage = "SIM specification cannot exceed 50 characters")]
        public string? Sim { get; set; }

        [StringLength(500, ErrorMessage = "Other specifications cannot exceed 500 characters")]
        public string? Other { get; set; }

        public List<int>? SelectedAttributeIds { get; set; }

        public List<PhoneAttributeMappingDto> AttributeMappings { get; set; } = new();
    }
} 