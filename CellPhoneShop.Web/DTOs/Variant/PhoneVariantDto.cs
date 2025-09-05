namespace CellPhoneShop.Web.DTOs.Variant;

public class PhoneVariantDto
{
    public int VariantId { get; set; }
    public int PhoneId { get; set; }
    public int? ColorId { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Sku { get; set; }
    public int Status { get; set; }
    public bool IsDefault { get; set; }
    public List<VariantAttributeMappingDto> VariantAttributes { get; set; } = new();
}

public class CreatePhoneVariantDto
{
    public int PhoneId { get; set; }
    public int? ColorId { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int Status { get; set; }
    public bool IsDefault { get; set; }
}

public class VariantAttributeDto
{
    public int VariantAttributeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<VariantAttributeValueDto> Values { get; set; } = new();
}

public class VariantAttributeValueDto
{
    public int ValueId { get; set; }
    public int VariantAttributeId { get; set; }
    public string Value { get; set; } = string.Empty;
} 