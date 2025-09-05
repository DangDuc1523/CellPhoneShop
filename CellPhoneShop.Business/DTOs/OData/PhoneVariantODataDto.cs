using System.ComponentModel.DataAnnotations;

namespace CellPhoneShop.Business.DTOs.OData
{
    public class PhoneVariantODataDto
    {
        [Key]
        public int VariantId { get; set; }
        public int PhoneId { get; set; }
        public int? ColorId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Sku { get; set; }
        public int Status { get; set; }
        public bool IsDefault { get; set; }
        public DateTime? CreatedAt { get; set; }
        
        // Phone information
        public string PhoneName { get; set; } = string.Empty;
        public string? PhoneDescription { get; set; }
        public decimal PhoneBasePrice { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public int BrandId { get; set; }
        
        // Color information
        public string? ColorName { get; set; }
        public string? ColorImageUrl { get; set; }
        
        // Calculated fields
        public decimal DiscountPercentage => PhoneBasePrice > 0 ? Math.Round(((PhoneBasePrice - Price) / PhoneBasePrice) * 100, 2) : 0;
        public bool IsInStock => Stock > 0;
        public bool IsActive => Status == 1;
        
        // Navigation properties for OData expand
        public PhoneODataDto? Phone { get; set; }
        public ColorODataDto? Color { get; set; }
        public List<VariantAttributeODataDto> VariantAttributes { get; set; } = new();
    }
    
    public class PhoneODataDto
    {
        [Key]
        public int PhoneId { get; set; }
        public string PhoneName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public List<PhoneAttributeODataDto> PhoneAttributes { get; set; } = new();
    }
    
    public class ColorODataDto
    {
        [Key]
        public int ColorId { get; set; }
        public string ColorName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
    
    public class VariantAttributeODataDto
    {
        [Key]
        public int MappingId { get; set; }
        public int VariantId { get; set; }
        public int AttributeId { get; set; }
        public int ValueId { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public string AttributeValue { get; set; } = string.Empty;
    }
    
    public class PhoneAttributeODataDto
    {
        [Key]
        public int MappingId { get; set; }
        public int PhoneId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
} 