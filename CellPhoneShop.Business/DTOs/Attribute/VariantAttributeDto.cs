namespace CellPhoneShop.Business.DTOs.Attribute;

public class VariantAttributeDto
{
    public int VariantAttributeId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public bool? IsDeleted { get; set; }
    public List<VariantAttributeValueDto> Values { get; set; } = new();
}

public class VariantAttributeValueDto
{
    public int ValueId { get; set; }
    public int VariantAttributeId { get; set; }
    public string Value { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public bool? IsDeleted { get; set; }
}

public class CreateVariantAttributeDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<string> Values { get; set; } = new();
}

public class UpdateVariantAttributeDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<VariantAttributeValueDto> Values { get; set; } = new();
}

public class CreateVariantAttributeValueDto
{
    public int VariantAttributeId { get; set; }
    public string Value { get; set; } = null!;
}

public class UpdateVariantAttributeValueDto
{
    public string Value { get; set; } = null!;
} 