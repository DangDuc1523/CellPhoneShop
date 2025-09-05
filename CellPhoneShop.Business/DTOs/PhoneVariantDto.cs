using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Business.DTOs
{
    public class PhoneVariantDto
    {
        public int VariantId { get; set; }
        public int? PhoneId { get; set; }
        public int? ColorId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Sku { get; set; }
        public int Status { get; set; }
        public bool? IsDefault { get; set; }
    }

    public class CreatePhoneVariantDto
    {
        [Required(ErrorMessage = "Phone ID is required")]
        public int PhoneId { get; set; }

        [Required(ErrorMessage = "Color ID is required")]
        public int ColorId { get; set; }


        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0")]
        public int Stock { get; set; }

        [StringLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
        public string? Sku { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Status must be greater than or equal to 0")]
        public int Status { get; set; }

        public bool? IsDefault { get; set; }
    }

    public class UpdatePhoneVariantDto
    {

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0")]
        public int Stock { get; set; }

        [StringLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
        public string? Sku { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Status must be greater than or equal to 0")]
        public int Status { get; set; }

        public bool? IsDefault { get; set; }
    }
} 