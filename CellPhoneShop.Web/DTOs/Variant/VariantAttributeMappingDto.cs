namespace CellPhoneShop.Web.DTOs.Variant;

public class VariantAttributeMappingDto
{
    public int VariantId { get; set; }
    public int ValueId { get; set; }
    public string AttributeName { get; set; } = string.Empty;
    public string AttributeValue { get; set; } = string.Empty;
}

public class CreateVariantAttributeMappingDto
{
    public int VariantId { get; set; }
    public int ValueId { get; set; }
} 